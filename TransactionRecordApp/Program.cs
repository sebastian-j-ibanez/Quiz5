using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionRecordApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Setup DbContext from appsettings.
var connStr = builder.Configuration.GetConnectionString("TransactionsDb");
builder.Services.AddDbContext<TransactionContext>(options => options.UseSqlServer(connStr));

// Add authentication.
builder.Services.AddAuthentication();

// Add identity to app services.
builder.Services.AddIdentity<User, IdentityRole>(options => {
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireDigit = true;
}).AddEntityFrameworkStores<TransactionContext>().AddDefaultTokenProviders();

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

// Setup admin user.
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
	await TransactionContext.CreateAdminUser(scope.ServiceProvider);
}

app.Run();
