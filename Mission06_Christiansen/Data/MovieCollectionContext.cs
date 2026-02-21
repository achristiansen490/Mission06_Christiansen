using Microsoft.EntityFrameworkCore;
using Mission06_Christiansen.Models;

namespace Mission06_Christiansen.Data;

public class MovieCollectionContext : DbContext
{
    public MovieCollectionContext(DbContextOptions<MovieCollectionContext> options)
        : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Explicit table names (matches DB)
        modelBuilder.Entity<Movie>().ToTable("Movies");
        modelBuilder.Entity<Category>().ToTable("Categories");

        // Relationship: Movies.CategoryId -> Categories.CategoryId
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Category)
            .WithMany(c => c.Movies)
            .HasForeignKey(m => m.CategoryId);
    }
}