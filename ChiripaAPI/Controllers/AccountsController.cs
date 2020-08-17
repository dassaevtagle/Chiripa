using System;
using System.Threading.Tasks;
using ChiripaAPI.Models;
using ChiripaAPI.Services.Infrastructure;
using ChiripaAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChiripaAPI.Controllers
{
    [Route("api/Account/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IUsuario _usuarioRepo;

        public AccountsController(UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager,
               IConfiguration config, 
                IUsuario usuarioRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _usuarioRepo = usuarioRepo;
        }

        // POST api/Account/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginVM loginVM) 
        {
            if (await _usuarioRepo.UserExists(loginVM) == false)
            {
                return BadRequest($"{loginVM.Email} no está registrado.");
            }

            var user = await _userManager.FindByNameAsync(loginVM.Email);
                
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, false);

            if(!result.Succeeded)
            {
                return BadRequest("Contraseña incorrecta.");
            }

            var tokens = await _usuarioRepo.GenerateTokens(loginVM);

            return Created("", tokens);

        }
        
        // POST api/Account/Registro
        [HttpPost]
        public async Task<IActionResult> Registro([FromBody] RegistroUsuarioVM usuarioVM) 
        {
            try
            {
                if (usuarioVM.Password != usuarioVM.ConfirmarPassword)
                return BadRequest("Las contraseñas no coinciden");

                if(_usuarioRepo.UserExists(usuarioVM) == true)
                return BadRequest($"{usuarioVM.Email} ya está registrado.");

                if (_usuarioRepo.RegistrarUsuario(usuarioVM))
                {
                    var tokens = await _usuarioRepo.GenerateTokens(usuarioVM);

                    return Created("", tokens);
                }

                return BadRequest("Ocurrió un error al registrar al usuario.");

            }
            
            catch (Exception ex)
            {
                return BadRequest($"Error en el método de registrar por: {ex}");
            }
      
        }
        

        // POST api/Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


    }
}