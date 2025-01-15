using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models;

public class Product
{
    [Key]
    [Required]
    [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Code must be in format XXXX-XXXX")]
    public required string Code { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}
