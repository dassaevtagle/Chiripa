using System;
using System.Threading.Tasks;
using ChiripaAPI.ViewModels;

namespace ChiripaAPI.Services.Infrastructure
{
    public interface IUsuario 
    {
        bool UserExists(RegistroUsuarioVM usuarioVM);
        Task<bool> UserExists(LoginVM usuarioVM);
        bool RegistrarUsuario(RegistroUsuarioVM usuarioVM);
        Task<Object> GenerateTokens(LoginVM loginVM);
        Task<Object> GenerateTokens(RegistroUsuarioVM usuarioVM);
    }
}