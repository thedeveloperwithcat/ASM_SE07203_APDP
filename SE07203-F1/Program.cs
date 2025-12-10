using Microsoft.EntityFrameworkCore;
using SE07203_F1.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 2. Cấu hình Cache (BẮT BUỘC ĐỂ SESSION CHẠY KHÔNG LỖI)
builder.Services.AddDistributedMemoryCache();

// 3. Cấu hình Session
builder.Services.AddSession(options =>
{
    // Tăng thời gian chờ lên 30 phút để không bị logout liên tục khi test
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 5. Kết nối Database SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=Se07203.db") // Fallback nếu không tìm thấy trong appsettings
);

var app = builder.Build();

// --- QUAN TRỌNG: TỰ ĐỘNG TẠO DATABASE NẾU CHƯA CÓ ---
// Đoạn này giúp fix lỗi AccountId1 bằng cách tạo DB mới khớp với Code
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated(); // Tự động tạo file .db mới
    }
    catch (Exception ex)
    {
        Console.WriteLine("Lỗi tạo Database: " + ex.Message);
    }
}

// 6. Configure Localization
var supportedCultures = new[] { new CultureInfo("vi-VN"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures.ToList(),
    SupportedUICultures = supportedCultures.ToList()
};
app.UseRequestLocalization(localizationOptions);

// 7. Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Kích hoạt Session TRƯỚC Routing
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

//using Microsoft.EntityFrameworkCore;
//using SE07203_F1.Data;
//using System.Globalization;
//using Microsoft.AspNetCore.Localization;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//// Localization services (cookie-based)
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite( 
//        builder.Configuration.GetConnectionString("DefaultConnection")
//    )
//);

////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////    options.UseSqlite(connectionString));

//builder.Services.AddSession(
//    options =>
//    {
//        options.IdleTimeout = TimeSpan.FromMinutes(2);
//    }
//);

//var app = builder.Build();

//// Configure request localization before other middlewares that use culture.
//var supportedCultures = new[] { new CultureInfo("vi-VN"), new CultureInfo("en-US") };
//var localizationOptions = new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("vi-VN"),
//    SupportedCultures = supportedCultures.ToList(),
//    SupportedUICultures = supportedCultures.ToList()
//};
//app.UseRequestLocalization(localizationOptions);

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseSession();
//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapRazorPages();

//app.Run();

