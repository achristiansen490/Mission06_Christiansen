using Microsoft.EntityFrameworkCore;
using Mission06_Christiansen.Data;

var builder = WebApplication.CreateBuilder(args);

//
// 1) Add MVC services (Controllers + Views)
//
builder.Services.AddControllersWithViews();

//
// 2) Register the EF Core DbContext and point it at the PROVIDED SQLite database
//    (Connection string lives in appsettings.json under "MovieConnection")
//
builder.Services.AddDbContext<MovieCollectionContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieConnection")));

var app = builder.Build();

//
// IMPORTANT (Mission 7):
// - Do NOT run DbSeeder.Seed(...)
// - Do NOT call db.Database.Migrate()
// We are using the provided database as-is.
//

//
// 3) Configure the HTTP request pipeline
//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // Serves images/css/js from wwwroot
app.UseRouting();

app.UseAuthorization();

//
// 4) Default route: Home/Index
//
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();