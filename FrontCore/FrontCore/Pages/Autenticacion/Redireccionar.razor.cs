using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FrontCore.Pages.Autenticacion
{
    public partial class Redireccionar
    {
        [Inject]
        private NavigationManager navManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> ProviederStateAuth { get; set; }

        bool noAutorizado { get; set; } = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var estadoAutorizacion = await ProviederStateAuth;

                if (estadoAutorizacion.User == null)
                {
                    var returnUrl = navManager.ToBaseRelativePath(navManager.Uri);

                    var redirectUri = string.IsNullOrEmpty(returnUrl) ? "Acceder" : $"Acceder?returnUrl={returnUrl}";

                    navManager.NavigateTo(redirectUri, true);
                }
                else
                {
                    noAutorizado = true;
                }
            }
            
        }
    }
}
