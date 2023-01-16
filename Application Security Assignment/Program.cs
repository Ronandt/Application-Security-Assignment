using Application_Security_Assignment.Data.Database.WebApp_Core_Identity.Model;
using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Identity;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddDbContext<AuthDbContext>(options =>

{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnectionString"));
});
builder.Services.Configure<IdentityOptions>(options => { 
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}


);
builder.Services.AddScoped<IImageService, ImageService>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    
}).AddEntityFrameworkStores<AuthDbContext>().AddErrorDescriber<ApplicationErrorDescriber>();
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
