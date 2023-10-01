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
        public async Task<IActionResult> Crear()
        {
            return View();
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