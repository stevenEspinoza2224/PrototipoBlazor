using FrontCore.Helpers;
using FrontCore.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model;
using System.Web;

namespace FrontCore.Pages.Autenticacion
{
    public partial class IniciarSesion
    {
        private UsuarioLogin UsuarioAutenticacion = new();
        public bool procesando { get; set; } = false;
        public bool MostrarErroresRegistro { get; set; }
        public List<string> Errores { get; set; } = new();
        public string? UrlRetorno { get; set; }
        [Inject]
        public IServicioAutenticacion ServicioAutenticacion { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        private async Task AccesoUsuario()
        {
            MostrarErroresRegistro = false;

            procesando = true;

            var resultado = await ServicioAutenticacion.Acceder(UsuarioAutenticacion);

            if (resultado is not null && resultado.IsSuccess)
            {
                procesando = false;

                UrlRetorno = HttpUtility.ParseQueryString((new Uri(NavManager.Uri)).ToString())["url"];

                NavManager.NavigateTo(string.IsNullOrEmpty(UrlRetorno) ? "/" : $"/{UrlRetorno}");
                
                SucessToast($"Bienvenido {resultado?.Response?.Usuario?.NombreUsuario}");
            }
            else
            {
                procesando = false;

                Errores.Add(resultado.CurrentException);

                MostrarErroresRegistro = true;

                NavManager.NavigateTo("/iniciar-sesion");
            }
        }
        private async Task SucessToast(string Mensaje)
        {
            await JsRuntime.ToastrSuccess(Mensaje);
        }
        private async Task ErrorToast(string Mensaje)
        {
            await JsRuntime.ToastrError(Mensaje);
        }
    }
}
