using System.ComponentModel.DataAnnotations;

namespace SportStore.WebUI.Models;

public class OrderViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string DeliveryName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Address")]
    public string DeliveryAddress { get; set; } = string.Empty;

    [Required]
    [Display(Name = "City")]
    public string DeliveryCity { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Postal Code")]
    public string DeliveryPostalCode { get; set; } = string.Empty;
} 