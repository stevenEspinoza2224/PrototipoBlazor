using Model;
using Model.Configuration;

namespace FrontCore.Services.IServices
{
    public interface IServicioAutenticacion
    {
        Task<ResponseGeneric<bool>> RegistrarUsuario(UsuarioRegistro usuarioRegistro);
        Task<ResponseGeneric<UsuarioLoginRespuesta>> Acceder(UsuarioLogin usuarioLogin);
        Task CerrarSesion();
    }
}
