using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TCA2023v2.Models;
using TCA2023v2_DataAccess;

namespace TCA2023v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ICategoriesRepository repo)
        {
            _logger = logger;


            repo.AddCategory(new TCA2023v2_Domain.CategoryType() { Id = 1, Title = "test 1" });
            repo.AddCategory(new TCA2023v2_Domain.CategoryType() { Id = 2, Title = "test 2" });
            repo.AddCategory(new TCA2023v2_Domain.CategoryType() { Id = 3, Title = "test 3" });
        }

        public IActionResult Index()
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