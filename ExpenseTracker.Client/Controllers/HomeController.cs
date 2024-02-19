using ExpenseTracker.Client.Models;
using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

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


        public IActionResult Register()
        {
            // Your logic for the Register action
            return View();

        }


    
        #region CreateUser
        public async Task<IActionResult> CreateUser(RegisterRequestModel model)
        {
            try
            {

                model.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/User/account/register", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ModelState.AddModelError(string.Empty, "User with this Email already exists. Please login.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                return View(model);
            }
        }
        #endregion

       


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
