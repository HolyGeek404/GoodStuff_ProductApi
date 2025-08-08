namespace Model.DataAccess;

public static class Queries
{
    public static readonly string GetAllByType = "SELECT * FROM c WHERE c.Category = @category";
    public static readonly string GetSingleById= "SELECT * FROM c WHERE c.Category = @category AND c.ProductId = @id";
}