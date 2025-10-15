namespace GoodStuff.ProductApi.Domain.Products.Models;

public class Cpu : BaseProduct
{
    public string? Family { get; set; }
    public string? Series { get; set; }
    public string? Socket { get; set; }
    public string? SupportedChipsets { get; set; }
    public string? RecommendedChipset { get; set; }
    public string? Architecture { get; set; }
    public string? Frequency { get; set; }
    public string? Cores { get; set; }
    public string? Threads { get; set; }
    public string? UnlockedMultiplayer { get; set; }
    public string? CacheMemory { get; set; }
    public string? IntegratedGpu { get; set; }
    public string? IntegratedGpuModel { get; set; }
    public string? SupportedRam { get; set; }
    public string? Lithography { get; set; }
    public string? Tdp { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? IncludedCooler { get; set; }
}