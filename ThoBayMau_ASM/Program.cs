using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(option =>
    {
        option.ClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
        option.ClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value;
    });

builder.Services.AddDbContext<ThoBayMau_ASMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ThoBayMau_ASMContext") ?? throw new InvalidOperationException("Connection string 'ThoBayMau_ASMContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(24);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
