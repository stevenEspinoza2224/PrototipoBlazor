using Blazored.LocalStorage;
using FrontCore.Helpers;
using FrontCore.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;
using Model;
using Model.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace FrontCore.Services
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _ProviderStateAuth;
        private readonly IConfiguration _configuration;
        public ServicioAutenticacion(HttpClient httpClient,
                                     ILocalStorageService localStorageService,
                                     AuthenticationStateProvider ProviderStateAuth,
                                     IConfiguration configuration)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _ProviderStateAuth = ProviderStateAuth;
            _configuration = configuration;
        }
        public async Task<ResponseGeneric<UsuarioLoginRespuesta>> Acceder(UsuarioLogin usuarioLogin)
        {
            var service = _configuration.GetSection("ServiceConfiguration").Get<ServiceConfiguration>()?["Backend"]
                          ?? throw new NullReferenceException($"No se encontro el serviceConfiguration de GMG.Middleware.ServiceConfig");

            var endpoint = service.Endpoints?.FirstOrDefault(elmt => elmt.Name == "Login");

            if (service is null && service?.BaseUri is null && endpoint is null && endpoint?.HttpMethod is null)
            {
                throw new NullReferenceException($"La configuracion del Servicio se encuentra incompleta"); ;
            }

            using var request = new HttpRequestMessage
            {
                Method = endpoint.HttpMethod,
                RequestUri = new Uri(service.BaseUri, endpoint.Url),
                Content = new StringContent(JsonConvert.SerializeObject(usuarioLogin), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var userLoginRespuesta = JObject.Parse(await response.Content.ReadAsStringAsync())?.SelectToken("response")?.ToObject<UsuarioLoginRespuesta>();

                await _localStorageService.SetItemAsync(Inicializar.Token_Local, userLoginRespuesta?.UserToken);

                await _localStorageService.SetItemAsync(Inicializar.Usuario_Local, userLoginRespuesta?.Usuario);

                ((AuthStateProvider)_ProviderStateAuth).NotificarUsuarioLogueado(userLoginRespuesta?.UserToken);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userLoginRespuesta?.UserToken);

                return new ResponseGeneric<UsuarioLoginRespuesta>(userLoginRespuesta, HttpStatusCode.OK);
            }
            var error = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());

            return new ResponseGeneric<UsuarioLoginRespuesta>(new UsuarioLoginRespuesta { Usuario = null, UserToken = "" }, HttpStatusCode.Unauthorized) { CurrentException=$"{error?.CurrentException}-{error?.Message}"};
        }

        public async Task<ResponseGeneric<bool>> RegistrarUsuario(UsuarioRegistro usuarioRegistro)
        {
            var service = _configuration.GetSection("ServiceConfiguration").Get<ServiceConfiguration>()?["Backend"]
                          ?? throw new NullReferenceException($"No se encontro el serviceConfiguration de Backend"); ;

            var endpoint = service.Endpoints?.FirstOrDefault(elmt => elmt.Name == "Registrar");

            if (service is null || service.BaseUri is null || endpoint is null || endpoint.HttpMethod is null)
            {
                throw new NullReferenceException($"La configuracion del Servicio se encuentra incompleta"); ;
            }

            using var request = new HttpRequestMessage
            {
                Method = endpoint.HttpMethod,
                RequestUri = new Uri(service.BaseUri, endpoint.Url),
                Content = new StringContent(JsonConvert.SerializeObject(usuarioRegistro), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await response.Content.ReadAsStringAsync();

                return new ResponseGeneric<bool>(true, HttpStatusCode.OK);
            }

            return new ResponseGeneric<bool>(false, HttpStatusCode.InternalServerError);
        }

        public async Task CerrarSesion()
        {
            await _localStorageService.RemoveItemAsync(Inicializar.Token_Local);

            await _localStorageService.RemoveItemAsync(Inicializar.Usuario_Local);

            ((AuthStateProvider)_ProviderStateAuth).NotificarUsuarioSalir();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
