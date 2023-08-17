using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Initialize()
        {
            if (await _roleManager.FindByNameAsync(SD.Admin) == null)
                await _roleManager.CreateAsync(new IdentityRole(SD.Admin));

            if (await _roleManager.FindByNameAsync(SD.Customer) == null)
                await _roleManager.CreateAsync(new IdentityRole(SD.Customer));

            var adminUser = new ApplicationUser
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111111111111",
                FirstName = "Admin1",
                LastName = "Admin1",
            };

            await _userManager.CreateAsync(adminUser, "Admin123#");
            await _userManager.AddToRoleAsync(adminUser, SD.Admin);
            await _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
                new Claim(ClaimTypes.Email, adminUser.Email),
                new Claim(ClaimTypes.GivenName, adminUser.FirstName),
                new Claim(ClaimTypes.Role, SD.Admin)
            });

            var customerUser = new ApplicationUser
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "22222222222222222",
                FirstName = "Customer1",
                LastName = "Customer1",
            };

            await _userManager.CreateAsync(customerUser, "Customer123#");
            await _userManager.AddToRoleAsync(customerUser, SD.Customer);
            await _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{customerUser.FirstName} {customerUser.LastName}"),
                new Claim(ClaimTypes.Email, customerUser.Email),
                new Claim(ClaimTypes.GivenName, customerUser.FirstName),
                new Claim(ClaimTypes.Role, SD.Customer)
            });
        }
    }
}
