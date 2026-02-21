using System.ComponentModel.DataAnnotations;

namespace Mission06_Christiansen.Models;

public class Movie
{
    [Key]
    public int MovieId { get; set; }

    // Matches DB: CategoryId is nullable
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // REQUIRED
    [Required]
    public string Title { get; set; } = string.Empty;

    // REQUIRED + rule: year >= 1888
    [Required]
    [Range(1888, 3000, ErrorMessage = "Year must be 1888 or later.")]
    public int Year { get; set; }

    public string? Director { get; set; }
    public string? Rating { get; set; }

    // REQUIRED (DB has NOT NULL integer 0/1)
    [Required]
    public bool Edited { get; set; }

    public string? LentTo { get; set; }

    // REQUIRED (DB has NOT NULL integer 0/1)
    [Required]
    public bool CopiedToPlex { get; set; }

    // DB allows null; Mission 6 had max 25, keep it (safe)
    [MaxLength(25)]
    public string? Notes { get; set; }
}