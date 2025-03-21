using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie = new CookieBuilder
    {
        Name = "Authentication",
        HttpOnly = true,
        Path = "/",
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };
    options.LoginPath = new PathString("/Authorize/Login");
    options.LogoutPath = new PathString("/Authorize/Logout");
    options.AccessDeniedPath = new PathString("/Error/403");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
