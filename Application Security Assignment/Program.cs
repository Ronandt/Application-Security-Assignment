using Application_Security_Assignment.Data.Database.WebApp_Core_Identity.Model;
using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Filters;
using Application_Security_Assignment.Identity;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.Filters.Add(new SessionAsyncFilter(new FilterSessionService()));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(FilterSessionService.SESSION_TIMEOUT_IN_SECONDS);
});

builder.Services.AddDbContext<AuthDbContext>(options =>

{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnectionString"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// NOTE: Stores to server memory
// TODO: Change to externals stores to allow horizontal scalling
builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<IdentityOptions>(options => { 
    //options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}


);
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IFilterSessionService, FilterSessionService>();
 builder.Services.AddScoped<ICryptographyService>(provider => new CryptographyService("FreshFarmMarket", "UserData"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.AllowedForNewUsers = LockoutConstants.ALLOWED_FOR_NEW_USERS;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(LockoutConstants.LOCKOUT_TIMESPAN_IN_MINUTES);
    options.Lockout.MaxFailedAccessAttempts = LockoutConstants.MAX_FAILED_ATTEMPTS;
}).AddEntityFrameworkStores<AuthDbContext>().AddErrorDescriber<ApplicationErrorDescriber>();
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/login");
builder.Services.AddDataProtection();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
