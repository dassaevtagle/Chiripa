using System.Data.Common;
using System;
using System.Linq;
using ChiripaAPI.Data;
using ChiripaAPI.Services.Infrastructure;
using ChiripaAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using ChiripaAPI.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ChiripaAPI.Services.Repositories
{
    public class UsuarioRepo : IUsuario
    {
        private readonly ChiripaDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public UsuarioRepo(ChiripaDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public bool UserExists(RegistroUsuarioVM usuarioVM)
        {
            if(_db.Users.Any( u => u.Email == usuarioVM.Email))
            {
                return true;
            }

            return false;
        
        }
        public async Task<bool> UserExists(LoginVM loginVM)
        {
            var user = await _userManager.FindByNameAsync(loginVM.Email);
            
            if(user != null)
            {
                return true;
            }

            return false;
        }

        public bool RegistrarUsuario(RegistroUsuarioVM usuarioVM)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Nombre = usuarioVM.Nombre,
                    UserName = usuarioVM.Email,
                    Email = usuarioVM.Email
                };

                var result = _userManager.CreateAsync(user, usuarioVM.Password).GetAwaiter().GetResult();
                
                if (!result.Succeeded)
                {
                    return false;
                }

                var userRegistered = _db.Users.Single(u => u.Email == usuarioVM.Email);

                _userManager.AddToRoleAsync(userRegistered, "Default").GetAwaiter().GetResult();
                {
                    return true;
                }
            }

            catch(Exception)
            {
                return false;
            }
        }

        public async Task<Object> GenerateTokens(LoginVM loginVM)
        {

            var user = await _userManager.FindByNameAsync(loginVM.Email);
                
            // Create the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Audience"],
                claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds);

            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                email = user.Email
            };

            return results;
            
        }

        public async Task<object> GenerateTokens(RegistroUsuarioVM usuarioVM)
        {
            var user = await _userManager.FindByNameAsync(usuarioVM.Email);
                
            // Create the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Audience"],
                claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds);

            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                email = user.Email
            };

            return results;
                        
        }
    }
}