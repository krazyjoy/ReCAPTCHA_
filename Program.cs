using Microsoft.EntityFrameworkCore;
using ReCAPTCHA.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ImageDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("ReCAPTCHA"),
     builder =>
     {
         builder.EnableRetryOnFailure(20, TimeSpan.FromSeconds(10), null);
     });


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

