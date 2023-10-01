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