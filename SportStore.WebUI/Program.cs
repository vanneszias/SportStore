using Microsoft.EntityFrameworkCore;
using SportStore.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Add DbContext configuration
builder.Services.AddDbContext<SportStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddInfrastructure();

builder.Services.AddDefaultIdentity<SportStore.Infrastructure.Data.ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<SportStore.Infrastructure.Data.SportStoreDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageCatalog", policy =>
        policy.RequireClaim("CanManageCatalog", "true"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseStatusCodePagesWithReExecute("/Home/Error");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SportStore.Infrastructure.Data.SportStoreDbContext>();
    db.Database.Migrate();

    // User and claim seeding
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SportStore.Infrastructure.Data.ApplicationUser>>();
    var adminEmail = "admin@sportstore.com";
    var adminPassword = "Admin123!";
    var userEmail = "user@sportstore.com";
    var userPassword = "User123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new SportStore.Infrastructure.Data.ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddClaimAsync(adminUser, new Claim("CanManageCatalog", "true"));
    }
    var regularUser = await userManager.FindByEmailAsync(userEmail);
    if (regularUser == null)
    {
        regularUser = new SportStore.Infrastructure.Data.ApplicationUser
        {
            UserName = userEmail,
            Email = userEmail,
            FirstName = "Regular",
            LastName = "User",
            BirthDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        await userManager.CreateAsync(regularUser, userPassword);
        // No claim for regular user
    }
}

app.Run();
