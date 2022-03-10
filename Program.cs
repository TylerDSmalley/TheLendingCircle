using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Data;
using TheLendingCircle.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DB services to the container.
//var connectionString = builder.Configuration.GetConnectionString("LendingCircleContext");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LendingCircleContext")));
//Local DB version
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Set to true if we implement email confirmation
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddRazorPages();


// //Switch between local sqlite for dev and SqlServer for production
// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddDbContext<ApplicationDbContext>(options =>
//         options.UseSqlite(builder.Configuration.GetConnectionString("ApplicationDbContext")));
// }
// else
// {
//     builder.Services.AddDbContext<ApplicationDbContext>(options =>
//         options.UseSqlServer(builder.Configuration.GetConnectionString("LendingCircleContext")));
// }

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(6);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

if(userManager != null && roleManager != null) {
    SeedUsersAndRoles(userManager, roleManager);
}

void SeedUsersAndRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
    string[] roleNamesList = new string[] { "User", "Admin" };
    foreach (string roleName in roleNamesList) {

        //FIX ME: Function throws NullReference here!!
        if(!roleManager.RoleExistsAsync(roleName).Result) {
            IdentityRole role = new IdentityRole();
            role.Name = roleName;
            IdentityResult result = roleManager.CreateAsync(role).Result;
        }
    }

    string adminEmail = "admin@admin.com";
    string adminPass = "Password1";
    if (userManager.FindByNameAsync(adminEmail).Result == null) {
        ApplicationUser user = new ApplicationUser();
        user.FirstName = "John";
        user.LastName = "Smith";
        user.Address = "123 Rue Peel";
        user.PostalCode = "H2K 2H2";
        user.City = "Montreal";
        user.Province = "Quebec";
        user.UserPhotoPath = "No Photo";
        user.UserName = adminEmail;
        user.Email = adminEmail;
        user.EmailConfirmed = true;

        IdentityResult result = userManager.CreateAsync(user, adminPass).Result;
        
        if (result.Succeeded) {
            IdentityResult result1 = userManager.AddToRoleAsync(user, "Admin").Result;
            if(result1.Succeeded) {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            } else {
                //log error
            }
            
        }
    }
}

app.MapRazorPages();

app.Run();
