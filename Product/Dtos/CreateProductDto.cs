// Create Product Dto
using System.ComponentModel.DataAnnotations;
public class CreateProductDto
{

    [Required(ErrorMessage = "Image is missing.")]
    public string Image { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is missing.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "SizeIds is missing.")]
    public List<Guid> SizeIds { get; set; }

    [Required(ErrorMessage = "ColorIds is missing.")]
    public List<Guid> ColorIds { get; set; }

    [Required(ErrorMessage = "Material is missing.")]
    public Material Material { get; set; }

    [Required(ErrorMessage = "Quantity is missing.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is missing.")]
    public decimal Price { get; set; }

}