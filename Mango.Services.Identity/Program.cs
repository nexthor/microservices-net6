using Mango.Services.Identity;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllersWithViews();

services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuring IdentityServer
services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.EmitStaticAudienceClaim = true;
    // SD = Static Detail
}).AddInMemoryIdentityResources(SD.IdentityResources) // Identity Resources
    .AddInMemoryApiScopes(SD.ApiScopes) // Identity Api Scopes
    .AddInMemoryClients(SD.Clients) // Clients
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential();

services.AddScoped<IDbInitializer, DbInitializer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();

dbInitializer.Initialize().GetAwaiter().GetResult();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
