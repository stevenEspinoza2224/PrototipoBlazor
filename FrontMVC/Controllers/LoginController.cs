using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net.Http;

namespace FrontMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private HttpClient _httpClient;
        private Service? service;
        public LoginController(ILogger<LoginController> logger,
                               HttpClient httpClient,
                               IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            service = configuration.GetSection("ServiceConfiguration").Get<ServiceConfiguration>()?["Backend"];
        }
        public IActionResult HomeLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarSesion(UsuarioLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                Model.Configuration.Endpoint endpoint = service?.Endpoints?["Login"] ?? throw new NullReferenceException("No se encontró el Endpoint");

                var respuesta = await _httpClient.PostAsJsonAsync($"{service.BaseUri}{endpoint.Url}", new {userName= userLogin.NombreUsuario,password=userLogin.Password});

                var token = JObject.Parse(await respuesta.Content.ReadAsStringAsync())?.SelectToken("token")?.ToObject<string>();

                // Guardar el token en la sesión
                HttpContext.Session.SetString("Token", token ?? "");

                return RedirectToAction("Index", "Home");
            }

            return View("HomeLogin");
        }
    }
}
