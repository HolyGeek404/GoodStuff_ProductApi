using Autofac;
using Azure.Identity;
using GoodStuff_DomainModels.Models.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Model.DataAccess;
using Model.Services;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddTransient<IProductDaoFactory,  ProductDaoFactory>();
        
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterType<GpuDao>().Keyed<IProductDao>("GPU");
            containerBuilder.RegisterType<CpuDao>().Keyed<IProductDao>("CPU");
            containerBuilder.RegisterType<CoolerDao>().Keyed<IProductDao>("COOLER");
        });
        
        return services;
    }

    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(BaseProductDao).Assembly));

        return services;
    }

    public static IServiceCollection AddAzureConfig(this IServiceCollection services, IConfigurationManager configuration)
    {
        var azureAd = configuration.GetSection("AzureAd");
        configuration.AddAzureKeyVault(new Uri(azureAd["KvUrl"]), new DefaultAzureCredential());
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(azureAd);

        return services;
    }

    public static IServiceCollection AddDataBaseConfig(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddSingleton(s => new CosmosClient(configuration.GetConnectionString("CosmosDB")));
        
        return services;
    }

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var tenantId = configuration.GetSection("AzureAd")["TenantId"];
        var swaggerScope = configuration.GetSection("Swagger")["Scope"];
        var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";

        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodStuff Product Api Swagger", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "OAuth2.0 Auth Code with PKCE",
                    Name = "oauth2",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),//token end point
                            Scopes = new Dictionary<string, string>
                                {
                                    { $"{swaggerScope}", "Swagger - Local testing" },
                                }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { $"{swaggerScope}" }
                    }
                });
            });

        return services;
    }
}