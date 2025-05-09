using Microsoft.AspNetCore.Mvc;
using SportStore.Domain.Entities;
using SportStore.Infrastructure.Data;
using SportStore.WebUI.Models;

namespace SportStore.WebUI.Controllers;

public class OrderController : Controller
{
    private readonly SportStoreDbContext _context;
    private const string CartKey = "cart";

    public OrderController(SportStoreDbContext context)
    {
        _context = context;
    }

    private ShoppingCart? GetCart()
    {
        return HttpContext.Session.GetJson<ShoppingCart>(CartKey);
    }

    private void ClearCart()
    {
        HttpContext.Session.Remove(CartKey);
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (cart == null || !cart.Lines.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }
        return View(new OrderViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(OrderViewModel vm)
    {
        var cart = GetCart();
        if (cart == null || !cart.Lines.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        // For now, use a placeholder UserId (to be replaced with real user after Identity setup)
        var order = new Order
        {
            UserId = "guest",
            DeliveryName = vm.DeliveryName,
            DeliveryAddress = vm.DeliveryAddress,
            DeliveryCity = vm.DeliveryCity,
            DeliveryPostalCode = vm.DeliveryPostalCode,
            OrderDate = DateTime.UtcNow,
            OrderLines = cart.Lines.Select(l => new OrderLine
            {
                ProductId = l.ProductId,
                ProductName = l.ProductName,
                PriceAtOrder = l.Price,
                Quantity = l.Quantity
            }).ToList()
        };
        _context.Orders.Add(order);
        _context.SaveChanges();
        ClearCart();
        return RedirectToAction("Confirmation", new { id = order.Id });
    }

    public IActionResult Confirmation(int id)
    {
        var order = _context.Orders
            .Where(o => o.Id == id)
            .Select(o => new
            {
                o.Id,
                o.DeliveryName,
                o.DeliveryAddress,
                o.DeliveryCity,
                o.DeliveryPostalCode,
                o.OrderDate,
                Lines = o.OrderLines.Select(l => new { l.ProductName, l.PriceAtOrder, l.Quantity })
            })
            .FirstOrDefault();
        if (order == null) return NotFound();
        return View(order);
    }
} 