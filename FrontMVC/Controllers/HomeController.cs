using FrontMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Configuration;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace FrontMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _httpClient;
        private Service? service;

        public HomeController(ILogger<HomeController> logger,
                              HttpClient httpClient,
                              IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            service = configuration.GetSection("ServiceConfiguration").Get<ServiceConfiguration>()?["Backend"];
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Model.Configuration.Endpoint endpoint = service?.Endpoints?["Lista"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var Clientes = JObject.Parse(await _httpClient.GetStringAsync($"{service.BaseUri}{endpoint.Url}")).SelectToken("response")?.ToObject<List<Cliente>>();

            return View(Clientes);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                Model.Configuration.Endpoint endpoint = service?.Endpoints?["Guardar"] ?? throw new NullReferenceException("No se encontró el Endpoint");

                var respuesta = await _httpClient.PostAsJsonAsync($"{service.BaseUri}{endpoint.Url}", cliente);

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Model.Configuration.Endpoint endpoint = service?.Endpoints?["Obtener"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var Cliente = JObject.Parse(await _httpClient.GetStringAsync($"{service.BaseUri}{endpoint.Url}{id}"))?.SelectToken("response")?.ToObject<Cliente>();

            return View(Cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                Model.Configuration.Endpoint endpoint = service?.Endpoints?["Editar"] ?? throw new NullReferenceException("No se encontró el Endpoint");

                var respuesta = await _httpClient.PutAsJsonAsync($"{service.BaseUri}{endpoint.Url}", cliente);

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Model.Configuration.Endpoint endpoint = service?.Endpoints?["Obtener"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var Cliente = JObject.Parse(await _httpClient.GetStringAsync($"{service.BaseUri}{endpoint.Url}{id}"))?.SelectToken("response")?.ToObject<Cliente>();

            return View(Cliente);
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Model.Configuration.Endpoint endpoint = service?.Endpoints?["Obtener"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var Cliente = JObject.Parse(await _httpClient.GetStringAsync($"{service.BaseUri}{endpoint.Url}{id}"))?.SelectToken("response")?.ToObject<Cliente>();

            return View(Cliente);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarCliente(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Model.Configuration.Endpoint endpointObtener = service?.Endpoints?["Obtener"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var Cliente = JObject.Parse(await _httpClient.GetStringAsync($"{service.BaseUri}{endpointObtener.Url}{id}"))?.SelectToken("response")?.ToObject<Cliente>();

            if (Cliente is null)
            {
                return View();
            }

            Model.Configuration.Endpoint endpointBorrar = service?.Endpoints?["Eliminar"] ?? throw new NullReferenceException("No se encontró el Endpoint");

            var respuesta = await _httpClient.DeleteAsync($"{service.BaseUri}{endpointBorrar.Url}{id}");

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}