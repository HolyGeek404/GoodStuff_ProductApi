namespace Model.DataAccess;

public static class Queries
{
    #region Basic
    private static readonly string BasicGetAllColumns = "c.Category, c.ProductId, c.ProductImg, c.Name, c.Price,";
    private static readonly string BasicGetAllByCategoryFrom = "FROM c WHERE c.Category = @category";
    private static readonly string BasicGetSingleById = $"SELECT * {BasicGetAllByCategoryFrom} AND c.ProductId = @id";
    #endregion  

    private static readonly string GetAllGpus = $"SELECT {BasicGetAllColumns} c.RecommendedPSUPower, c.MemoryBus, c.CoreRatio {BasicGetAllByCategoryFrom}";
    private static readonly string GetAllCpus = $"SELECT {BasicGetAllColumns} c.Socket, c.Architecture, c.TDP {BasicGetAllByCategoryFrom}";
    private static readonly string GetAllCoolers = $"SELECT {BasicGetAllColumns} c.Fans, c.RPMControll, c.Compatibility, c.HeatPipes {BasicGetAllByCategoryFrom}";

    private static readonly string GetSingleCpu = $"SELECT c.Name, c.Team, c.Price, c.Familiy, c.Series, c.Socket, c.SupportedChipsets, c.RecomendedChipset, c.Architecture, c.Frequency, c.Cores, c.Threads, c.UnlockedMultipler, c.CacheMemory, c.IntegredGpu, c.IntegredGpuModel, c.SupportedRam, c.Litography, c.TDP, c.AdditionalInfo, c.IncludedCooler, c.Warranty, c.ProductImg, c.Category, c.ProductId  {BasicGetAllByCategoryFrom} AND c.ProductId = @id";
    private static readonly string GetSingleGpu = $"SELECT c.ProductId, c.Name, c.Price, c.Team, c.GpuProcessorLine, c.PCIeCategory, c.MemorySize, c.MemoryCategory, c.MemoryBus, c.MemoryRatio, c.CoreRatio, c.CoresNumber, c.CoolingCategory, c.OutputsCategory, c.SupportedLibraries, c.PowerConnector, c.RecommendedPSUPower, c.Length, c.Width, c.Height, c.Warranty, c.ProducentCode, c.PgpCode, c.GpuProcessorName, c.Manufacturer, c.ProductImg, c.Category {BasicGetAllByCategoryFrom} AND c.ProductId = @id";
    private static readonly string GetSingleCooler = $"SELECT c.Name, c.Team, c.CoolerType, c.Compatibility, c.Size, c.HeatPipes, c.Fans, c.RPMControll, c.RMP, c.BearingType, c.FanSize, c.Connector, c.SupplyVoltage, c.SupplyCurrent, c.Highlight, c.MTBFLifetime, c.Height, c.Width, c.Depth, c.Weight, c.Warranty, c.ProducentCode, c.ProductImg, c.Price, c.Manufacture, c.Category, c.ProductId  {BasicGetAllByCategoryFrom} AND c.ProductId = @id";

    public static string GetAllByType(string type)
    {
        return type switch
        {
            "GPU" => GetAllGpus,
            "CPU" => GetAllCpus,
            "COOLER" => GetAllCoolers,
            _ => "",
        };
    }

    public static string GetSingleById(string type)
    {
        return type switch
        {
            "GPU" => GetSingleGpu,
            "CPU" => GetSingleCpu,
            "COOLER" => GetSingleCooler,
            _ => "",
        };
    }
}