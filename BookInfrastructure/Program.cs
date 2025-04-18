using BookDomain.Model;
using BookInfrastructure;
using BookInfrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDataPortServiceFactory<Book>, BookDataPortServiceFactory>();

// ������ MVC �� Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ������ �������� ���� �����
builder.Services.AddDbContext<BooksShopdatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� Identity
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

// ����������� ��������������
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

// ������������ ����� �� ������������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Customer>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    try
    {
        // ��������� ����� ����������� ����� �� ������������
        await IdentityRoless.SeedRolesAndAdminAsync(userManager, roleManager).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        // ��������� ������� (������� ��� ������� ���������, ���������, ILogger)
        Console.WriteLine($"������� ��� ����������� �����: {ex.Message}");
    }
}

// ������������ ������� ������� ��� production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// �������������� �� �����������
app.UseAuthentication();
app.UseAuthorization();

// �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // �������� .WithStaticAssets()

app.MapRazorPages();

app.Run();