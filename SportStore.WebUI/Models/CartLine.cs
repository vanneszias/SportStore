using SportStore.Domain.Entities;

namespace SportStore.WebUI.Models;

public class CartLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageURL { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
} 