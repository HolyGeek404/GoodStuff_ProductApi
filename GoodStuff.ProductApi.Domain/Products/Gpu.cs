using GoodStuff.ProductApi.Domain.Products;

public class Gpu : BaseProduct
{
    public override required string Category { get; set; } = ProductCategories.Cpu;
    public string? GpuProcessorLine { get; set; }
    public string? PcieType { get; set; }
    public string? MemorySize { get; set; }
    public string? MemoryType { get; set; }
    public string? MemoryBus { get; set; }
    public string? MemoryRatio { get; set; }
    public string? CoreRatio { get; set; }
    public string? CoresNumber { get; set; }
    public string? CoolingType { get; set; }
    public string? OutputsType { get; set; }
    public string? SupportedLibraries { get; set; }
    public string? PowerConnector { get; set; }
    public string? RecommendedPsuPower { get; set; }
    public string? Length { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public string? GpuProcessorName { get; set; }
    public string? Manufacturer { get; set; }
}