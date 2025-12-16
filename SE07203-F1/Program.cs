using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using SE07203_F1.Repositories; 
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Localization services (cookie-based)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Cấu hình Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
});


builder.Services.AddScoped<IStudentRepository, StudentRepository>();


var app = builder.Build();

// 2. Configure the HTTP request pipeline.

// Cấu hình ngôn ngữ
var supportedCultures = new[] { new CultureInfo("vi-VN"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures.ToList(),
    SupportedUICultures = supportedCultures.ToList()
};
app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();