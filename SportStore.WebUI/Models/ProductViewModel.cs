using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SportStore.WebUI.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    public string? ImageURL { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public IEnumerable<CategoryViewModel>? Categories { get; set; }

    public IFormFile? ImageFile { get; set; }
}

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
} 