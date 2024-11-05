using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Server connection with singleton lifetime
builder.Services.AddDbContext<GameLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

//GameSubject with singleton
builder.Services.AddSingleton<GameSubject>(provider =>
{
    var notifier = new GameSubject();
    notifier.Attach(new GameAddedObserver()); // Attach observer only once
    return notifier;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
