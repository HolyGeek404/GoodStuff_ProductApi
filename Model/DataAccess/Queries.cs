namespace Model.DataAccess;

public static class Queries
{
    #region Basic
    private static readonly string BasicColumns = "c.Category, c.ProductId, c.ProductImg, c.Name, c.Price,";
    private static readonly string BasicCategoryFrom = "FROM c WHERE c.Category = @category";
    private static readonly string BasicCategoryFromWithProductId = $"{BasicCategoryFrom} AND c.ProductId = @id";
    #endregion  

    #region Gpu
    private static readonly string GetAllGpusExtension = "c.RecommendedPSUPower, c.MemoryBus, c.CoreRatio";
    private static readonly string GetAllGpus = $"SELECT {BasicColumns} {GetAllGpusExtension} {BasicCategoryFrom}";
    private static readonly string GetSingleGpu = $"SELECT {BasicColumns} {GetAllGpusExtension} c.GpuProcessorLine, c.PCIeCategory, c.MemorySize, c.MemoryCategory, c.MemoryRatio, c.CoresNumber, c.CoolingCategory, c.OutputsCategory, c.SupportedLibraries, c.PowerConnector, c.Length, c.Width, c.Height, c.Warranty, c.ProducentCode, c.PgpCode, c.GpuProcessorName, c.Manufacturer, c.ProductImg {BasicCategoryFromWithProductId}";
    #endregion

    #region Cpu
    private static readonly string GetAllCpusExtension = "c.Socket, c.Architecture, c.TDP";
    private static readonly string GetAllCpus = $"SELECT {BasicColumns} {GetAllCpusExtension} {BasicCategoryFrom}";
    private static readonly string GetSingleCpu = $"SELECT {BasicColumns} {GetAllCpusExtension}, c.Familiy, c.Series, c.SupportedChipsets, c.RecomendedChipset, c.Frequency, c.Cores, c.Threads, c.UnlockedMultipler, c.CacheMemory, c.IntegredGpu, c.IntegredGpuModel, c.SupportedRam, c.Litography, c.AdditionalInfo, c.IncludedCooler, c.Warranty {BasicCategoryFromWithProductId}";
    #endregion

    #region Cooler
    private static readonly string GetAllCoolersExtension = "c.Fans, c.RPMControll, c.Compatibility, c.HeatPipes";
    private static readonly string GetAllCoolers = $"SELECT {BasicColumns} {GetAllCoolersExtension} {BasicCategoryFrom}";
    private static readonly string GetSingleCooler = $"SELECT {BasicColumns} {GetAllCoolersExtension}, c.CoolerType, c.Compatibility, c.Size, c.HeatPipes, c.Fans, c.RPMControll, c.RMP, c.BearingType, c.FanSize, c.Connector, c.SupplyVoltage, c.SupplyCurrent, c.Highlight, c.MTBFLifetime, c.Height, c.Width, c.Depth, c.Weight, c.Warranty, c.ProducentCode, c.Manufacture, {BasicCategoryFromWithProductId}";
    #endregion

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