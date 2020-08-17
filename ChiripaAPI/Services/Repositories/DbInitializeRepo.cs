using System.Linq;
using ChiripaAPI.Data;
using ChiripaAPI.Models;
using ChiripaAPI.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ChiripaAPI.Services.Repositories
{
    public class DbInitializeRepo : IDbInitialize
    {
        private readonly ChiripaDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public DbInitializeRepo(ChiripaDbContext db,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        public void Initialize()
        {
            if (!_db.Roles.Any(r => r.Name == "Administrador"))
                _roleManager.CreateAsync(new IdentityRole("Administrador")).GetAwaiter().GetResult();
            if (!_db.Roles.Any(r => r.Name == "Default"))
                _roleManager.CreateAsync(new IdentityRole("Default")).GetAwaiter().GetResult();
            
            if(!_db.Users.Any(r => r.Email == "rsamaniego819@gmail.com"))
            {
                string user = "rsamaniego819@gmail.com";
                string password =  _config["PasswordAdmin"];

                var userAdmin = new ApplicationUser 
                {
                    Nombre = "Romina Samaniego",
                    UserName = user,
                    Email = user,
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(userAdmin, password).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync(user).GetAwaiter().GetResult(), "Administrador")
                .GetAwaiter().GetResult();
            }
        }
    }
}