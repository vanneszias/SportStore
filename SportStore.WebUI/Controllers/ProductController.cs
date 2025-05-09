using Microsoft.AspNetCore.Mvc;
using SportStore.Application;
using SportStore.Domain.Entities;
using SportStore.WebUI.Models;
using SportStore.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SportStore.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly SportStoreDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductController(IProductService productService, SportStoreDbContext context, IWebHostEnvironment env)
    {
        _productService = productService;
        _context = context;
        _env = env;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    // GET: Product/Create
    [Authorize(Policy = "CanManageCatalog")]
    public IActionResult Create()
    {
        var vm = new ProductViewModel
        {
            Categories = _context.Categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }).ToList()
        };
        return View(vm);
    }

    // POST: Product/Create
    [Authorize(Policy = "CanManageCatalog")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel vm)
    {
        if (ModelState.IsValid)
        {
            string imageUrl = string.Empty;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.ImageFile.CopyToAsync(stream);
                }
                imageUrl = $"/images/products/{uniqueFileName}";
            }
            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description ?? string.Empty,
                Price = vm.Price,
                ImageURL = imageUrl,
                CategoryId = vm.CategoryId
            };
            await _productService.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }
        vm.Categories = _context.Categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }).ToList();
        return View(vm);
    }

    // GET: Product/Edit/5
    [Authorize(Policy = "CanManageCatalog")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        var vm = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageURL = product.ImageURL,
            CategoryId = product.CategoryId,
            Categories = _context.Categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }).ToList()
        };
        return View(vm);
    }

    // POST: Product/Edit/5
    [Authorize(Policy = "CanManageCatalog")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        if (ModelState.IsValid)
        {
            string imageUrl = vm.ImageURL ?? string.Empty;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.ImageFile.CopyToAsync(stream);
                }
                imageUrl = $"/images/products/{uniqueFileName}";
            }
            var product = new Product
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description ?? string.Empty,
                Price = vm.Price,
                ImageURL = imageUrl,
                CategoryId = vm.CategoryId
            };
            await _productService.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }
        vm.Categories = _context.Categories.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }).ToList();
        return View(vm);
    }

    // GET: Product/Delete/5
    [Authorize(Policy = "CanManageCatalog")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Product/Delete/5
    // This action is POST-only and is called by the Delete confirmation form. Do not access /Product/DeleteConfirmed directly.
    [Authorize(Policy = "CanManageCatalog")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Product/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }
} 