using Microsoft.EntityFrameworkCore;
using PonehRestaurantMenu.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// ✅ اضافه کردن MVC
builder.Services.AddControllersWithViews();

// ✅ تنظیم دیتابیس (SQLite برای لوکال، PostgreSQL برای Render)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Host="))
    {
        // یعنی ConnectionString مربوط به PostgreSQL هست
        options.UseNpgsql(connectionString);
    }
    else
    {
        // پیش‌فرض: SQLite
        options.UseSqlite(connectionString ?? "Data Source=orders.db");
    }
});

var app = builder.Build();

// ✅ اجرای Migration و SeedData فقط یک بار
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();      // جدول‌ها رو می‌سازه اگر وجود نداشته باشن
    SeedData.Initialize(db);    // داده‌های اولیه رو وارد می‌کنه
}

// ✅ Middleware ها
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ✅ مسیر پیش‌فرض
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
