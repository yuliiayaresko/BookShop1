using BookDomain.Model;
using BookInfrastructure;
using BookInfrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDataPortServiceFactory<Book>, BookDataPortServiceFactory>();

// Додаємо MVC та Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Додаємо контекст бази даних
builder.Services.AddDbContext<BooksShopdatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Налаштовуємо Identity
builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<BooksShopdatabaseContext>()
.AddDefaultTokenProviders();

// Налаштовуємо аутентифікацію
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

// Налаштування ролей та адміністратора
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Customer>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        // Викликаємо метод ініціалізації ролей та адміністратора
        await IdentityRoless.SeedRolesAndAdminAsync(userManager, roleManager).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        // Логування помилок (додайте свій механізм логування, наприклад, ILogger)
        Console.WriteLine($"Помилка при ініціалізації ролей: {ex.Message}");
    }
}

// Налаштування обробки помилок для production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Аутентифікація та авторизація
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизація
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Прибрано .WithStaticAssets()

app.MapRazorPages();

app.Run();