using CustomerWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomerWebSite.Controllers
{

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		NorthwindContext _context;
		public HomeController(ILogger<HomeController> logger, NorthwindContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();  //Index.cshtml
		}

		public IActionResult Privacy()
		{
			return View();  //Privacy.cshtml
		}
		public IActionResult Customers()
		{
			//return View(_context.Customers);  //Customers.cshtml
			NorthwindContext context = new NorthwindContext();
			return View(context.Customers);  //Customers.cshtml
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
