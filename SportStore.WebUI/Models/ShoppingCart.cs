namespace SportStore.WebUI.Models;

public class ShoppingCart
{
    public List<CartLine> Lines { get; set; } = new();

    public decimal Subtotal => Lines.Sum(l => l.Price * l.Quantity);
    public int TotalItems => Lines.Sum(l => l.Quantity);

    public void AddItem(CartLine line)
    {
        var existing = Lines.FirstOrDefault(l => l.ProductId == line.ProductId);
        if (existing != null)
        {
            existing.Quantity += line.Quantity;
        }
        else
        {
            Lines.Add(line);
        }
    }

    public void RemoveItem(int productId)
    {
        var line = Lines.FirstOrDefault(l => l.ProductId == productId);
        if (line != null)
        {
            Lines.Remove(line);
        }
    }

    public void Clear() => Lines.Clear();
} 