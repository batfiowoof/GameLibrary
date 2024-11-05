using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Връзката със сървъра е Singleton, защото искаме да имаме само една връзка с базата данни
builder.Services.AddDbContext<GameLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

//GameSubject with singleton
// Singleton се използва за да се гарантира, че ще имаме само един GameSubject, който ще бъде споделян от всички клиенти
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
