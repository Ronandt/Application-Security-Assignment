using Application_Security_Assignment.Data.Database.WebApp_Core_Identity.Model;
using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Filters;
using Application_Security_Assignment.Identity;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using Microsoft.Build.Framework;
using EllipticCurve.Utils;
using System.Configuration;

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
builder.Services.AddControllers();
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



builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/signin-google";
    
});
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IFilterSessionService, FilterSessionService>();
 builder.Services.AddScoped<ICryptographyService>(provider => new CryptographyService("FreshFarmMarket", "UserData"));
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<IEncoderService, EncoderService>();
builder.Services.AddScoped<PrepopulationService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.AllowedForNewUsers = LockoutConstants.ALLOWED_FOR_NEW_USERS;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(LockoutConstants.LOCKOUT_TIMESPAN_IN_MINUTES);
    options.Lockout.MaxFailedAccessAttempts = LockoutConstants.MAX_FAILED_ATTEMPTS;
}).AddEntityFrameworkStores<AuthDbContext>().AddErrorDescriber<ApplicationErrorDescriber>().AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(x =>
{
    x.ValidationInterval = TimeSpan.Zero;


});
builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/login";


});

builder.Services.AddDataProtection();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>(x => new EmailSenderService(builder.Configuration.GetSection("Smtp").Get<EmailCredentials>()));
builder.Services.AddScoped<SecurityFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();




app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

app.Run();
