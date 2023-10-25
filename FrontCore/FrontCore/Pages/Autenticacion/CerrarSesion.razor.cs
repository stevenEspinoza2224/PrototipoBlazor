using FrontCore.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace FrontCore.Pages.Autenticacion
{
    public partial class CerrarSesion
    {
        [Inject]
        public IServicioAutenticacion ServicioAutenticacion { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ServicioAutenticacion.CerrarSesion();
            NavManager.NavigateTo("/");
        }
    }
}
