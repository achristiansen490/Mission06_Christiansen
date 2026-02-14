using System.ComponentModel.DataAnnotations;

namespace Mission06_Christiansen.Models;

public class Movie
{
    [Key]
    public int MovieId { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int Year { get; set; }

    [Required]
    public string Director { get; set; } = string.Empty;

    [Required]
    public string Rating { get; set; } = string.Empty; // (G, PG, PG-13, R)

    // Not required:
    public bool? Edited { get; set; }   // yes/no, optional
    public string? LentTo { get; set; } // optional

    [MaxLength(25)]
    public string? Notes { get; set; }  // optional, max 25
}