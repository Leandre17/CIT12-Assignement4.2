namespace WebApi.Models;

public class ProductModel
{
    public string? Url { get; set; }
    public string Name { get; set; } = string.Empty;
    public double UnitPrice { get; set; }
    public string? ProductName { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int UnitsInStock { get; set; }
    public string? QuantityPerUnit { get; set; }
}
