// public list for t-shirt color 

using System.ComponentModel.DataAnnotations;

public class Color
{
    public Guid ColorId { get; set; }

    [Required(ErrorMessage = "Color value is missing.")]
    public string Value { get; set; } = string.Empty;
}