using System.ComponentModel.DataAnnotations;

namespace Mission06_Christiansen.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    public string CategoryName { get; set; } = string.Empty;

    // Navigation (optional but helpful)
    public List<Movie> Movies { get; set; } = new();
}