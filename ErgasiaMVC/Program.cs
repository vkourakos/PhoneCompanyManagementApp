using ErgasiaMVC.Data;
using ErgasiaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed roles
    var roles = new[] { nameof(Admin), nameof(Client), nameof(Seller) };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed Program
    if (!dbContext.Programs.Any())
    {
        dbContext.Programs.AddRange(
            new ErgasiaMVC.Models.Program { ProgramName = "Basic Plan", Benefits = "Basic coverage", Charge = 9.99m },
            new ErgasiaMVC.Models.Program { ProgramName = "Premium Plan", Benefits = "Full coverage", Charge = 19.99m }
        );
        await dbContext.SaveChangesAsync();
    }

    // Seed Phone
    if (!dbContext.Phones.Any())
    {
        dbContext.Phones.AddRange(
            new Phone { PhoneNumber = "6912345678", ProgramName = "Basic Plan" },
            new Phone { PhoneNumber = "6923456789", ProgramName = "Premium Plan" }
        );
        await dbContext.SaveChangesAsync();
    }

    // Seed Admin
    string adminEmail = "admin@admin.com";
    string adminPassword = "Admin123!";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Admin"
        };

        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, nameof(Admin));

        dbContext.Admins.Add(new Admin { Id = Guid.NewGuid().ToString(), UserId = adminUser.Id });
        await dbContext.SaveChangesAsync();
    }

    // Seed Client 
    string clientEmail = "client@client.com";
    string clientPassword = "Client123!";

    if (await userManager.FindByEmailAsync(clientEmail) == null)
    {
        var clientUser = new User
        {
            UserName = clientEmail,
            Email = clientEmail,
            EmailConfirmed = true,
            FirstName = "Client",
            LastName = "Client"
        };

        await userManager.CreateAsync(clientUser, clientPassword);
        await userManager.AddToRoleAsync(clientUser, nameof(Client));

        dbContext.Clients.Add(new Client
        {
            Id = Guid.NewGuid().ToString(),
            UserId = clientUser.Id,
            Afm = "123456789",
            PhoneNumber = "6912345678"
        });
        await dbContext.SaveChangesAsync();
    }

    // Seed Seller
    string sellerEmail = "seller@seller.com";
    string sellerPassword = "Seller123!";

    if (await userManager.FindByEmailAsync(sellerEmail) == null)
    {
        var sellerUser = new User
        {
            UserName = sellerEmail,
            Email = sellerEmail,
            EmailConfirmed = true,
            FirstName = "Seller",
            LastName = "Seller"
        };

        await userManager.CreateAsync(sellerUser, sellerPassword);
        await userManager.AddToRoleAsync(sellerUser, nameof(Seller));

        dbContext.Sellers.Add(new Seller { Id = Guid.NewGuid().ToString(), UserId = sellerUser.Id });
        await dbContext.SaveChangesAsync();
    }
}

app.Run();
