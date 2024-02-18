using ExpenseTracker.Client.Models;
using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace ExpenseTracker.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/User/account/login", model);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    // Set a local storage variable to indicate a successful login
                    TempData["LoginSuccess"] = "true";

                    return RedirectToAction("Dashboard");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Authentication failed
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
                else
                {
                    // Handle other response statuses
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
            }

            return View();
        }

        public IActionResult Dashboard()
        {
            // Your dashboard logic here
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
