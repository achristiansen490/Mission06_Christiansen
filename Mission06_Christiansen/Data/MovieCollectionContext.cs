using Microsoft.EntityFrameworkCore;
using Mission06_Christiansen.Models;

namespace Mission06_Christiansen.Data;

public class MovieCollectionContext(DbContextOptions<MovieCollectionContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
}