using System.ComponentModel.DataAnnotations;

namespace ChiripaAPI.ViewModels
{
    public class RegistroUsuarioVM
    {
        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmarPassword { get; set; }
    }
}