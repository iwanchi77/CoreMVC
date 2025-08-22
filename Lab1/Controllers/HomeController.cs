using System.Diagnostics;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		// GET: Home/Index
		[HttpGet]
        public IActionResult Index()
        {
             
			return View(); //Index.cshtml
		}

		// POST: Home/Index
		[HttpPost]
		public IActionResult Index(int x)
		{

			return View(); //Index.cshtml 會顯示Home/Index的POST結果
		}

		public IActionResult Privacy()
        {
			//int x = 0;
			//int y = 10;
			//int z = y / x; // This will throw a DivideByZeroException

			return View(); //Privacy.cshtml 會顯示Home/Privacy的內容
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
		{
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); //Error.cshtml 會顯示Home/Shared/Error的內容
		}
    }
}
