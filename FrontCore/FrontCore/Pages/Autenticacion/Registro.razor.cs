using FrontCore.Services.IServices;
using Microsoft.AspNetCore.Components;
using Model;

namespace FrontCore.Pages.Autenticacion
{
    public partial class Registro
    {
        private UsuarioRegistro UsuarioRegistro = new();
        public bool procesando { get; set; } = false;
        public bool MostrarErroresRegistro { get; set; }
        public List<string> Errores { get; set; } = new();

        [Inject]
        public IServicioAutenticacion ServicioAutenticacion { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        private async Task RegistrarUsuario()
        {
            MostrarErroresRegistro = false;

            procesando = true;

            var resultado = await ServicioAutenticacion.RegistrarUsuario(UsuarioRegistro);

            if (resultado is not null && resultado.Response == true)
            {
                procesando = false;
                NavManager.NavigateTo("/acceder");

            }
            else
            {
                procesando = false;
                Errores.Add(resultado.CurrentException);
                MostrarErroresRegistro = true;
            }
        }
    }
}
