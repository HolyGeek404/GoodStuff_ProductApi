using GoodStuff_DomainModels.Models.Products;

namespace GoodStuff.ProductApi.Application.Interfaces;

public interface IRepository
{
    public Task<IEnumerable<BaseProductModel>> GetAllByTypeAsync(string category);
    public Task<BaseProductModel> GetSingleById(string category, string id);
    public Task<BaseProductModel> CreateAsync(BaseProductModel entity);
    public Task<BaseProductModel> UpdateAsync(BaseProductModel entity);
    public Task DeleteAsync(BaseProductModel entity);
}