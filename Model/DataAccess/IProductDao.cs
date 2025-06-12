namespace Model.DataAccess;

public interface IProductDao
{
    Task<object?> GetAllProductsByType(string type);
}
