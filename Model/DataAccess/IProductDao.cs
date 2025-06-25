namespace Model.DataAccess;

public interface IProductDao
{
    Task<object?> GetAllProductsByType(string type);
    Task<object?> GetProductByIdQuery(string type, int id);
}
