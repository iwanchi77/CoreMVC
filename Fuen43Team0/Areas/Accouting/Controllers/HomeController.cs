using Microsoft.AspNetCore.Mvc;

namespace Fuen43Team0.Areas.Accouting.Controllers
{
	[Area("Accouting")]
	public class HomeController : Controller
	{
		
		public IActionResult Index()
		{
			return View();  // Views/Accouting/Home/Index.cshtml
		}


		public IActionResult Depreciation()
		{
			return View(); // Views/Accouting/Home/Depreciation.cshtml
		}

		public IActionResult WriteOff()
		{
			return View(); // Views/Accouting/Home/Depreciation.cshtml
		}
	}
}
