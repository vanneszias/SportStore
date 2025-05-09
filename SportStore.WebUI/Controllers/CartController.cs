using Microsoft.AspNetCore.Mvc;
using SportStore.Application;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private const string CartKey = "cart";

    public CartController(IProductService productService)
    {
        _productService = productService;
    }

    private ShoppingCart GetCart()
    {
        var cart = HttpContext.Session.GetJson<ShoppingCart>(CartKey) ?? new ShoppingCart();
        return cart;
    }

    private void SaveCart(ShoppingCart cart)
    {
        HttpContext.Session.SetJson(CartKey, cart);
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _productService.GetByIdAsync(productId);
        if (product == null) return NotFound();
        var cart = GetCart();
        cart.AddItem(new CartLine
        {
            ProductId = product.Id,
            ProductName = product.Name,
            ImageURL = product.ImageURL,
            Price = product.Price,
            Quantity = quantity
        });
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveItem(productId);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Clear()
    {
        var cart = GetCart();
        cart.Clear();
        SaveCart(cart);
        return RedirectToAction("Index");
    }
} 