namespace Model.DataAccess;

public static class Queries
{
    #region Basic
    private static readonly string BasicGetAllColumns = "c.Category, c.ProductId, c.ProductImg, c.Name,";
    private static readonly string BasicGetAllByCategoryFrom = "FROM c WHERE c.Category = @category";
    private static readonly string BasicGetSingleById = $"SELECT * {BasicGetAllByCategoryFrom} AND c.ProductId = @id";
    #endregion

    private static readonly string GetAllGpus = $"SELECT {BasicGetAllColumns} c.RecommendedPSUPower, c.MemoryBus, c.CoreRatio, c.Price {BasicGetAllByCategoryFrom}";
    private static readonly string GetAllCpus = $"SELECT {BasicGetAllColumns} c.Socket, c.Architecture, c.TDP, c.Price {BasicGetAllByCategoryFrom}";


    public static string GetAllByType(string type)
    {
        return type switch
        {
            "GPU" => GetAllGpus,
            "CPU" => GetAllCpus,
            _ => "",
        };
    }

    public static string GetSingleById()
    {
        return BasicGetSingleById;
    }
}