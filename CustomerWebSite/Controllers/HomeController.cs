using CustomerWebSite.Models;
using CustomerWebSite.ViewModels;
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

		// GET: /Home/Contact
		[HttpGet]
		public IActionResult Contact() 
		{

			return View();  //Contact.cshtml
		}

		// POST: /Home/Contact
		[HttpPost]
		[ValidateAntiForgeryToken] //防止跨站請求偽造攻擊 

		//Bind防止過度張貼攻擊
		public IActionResult Contact([Bind("Name,Email")]ContactViewModel cvm) //明列所有可以接收的欄位
		{
			if (ModelState.IsValid) //檢查表單欄位資料，通過Server端驗證
			{
				//TODO: 將聯絡人資料存入資料庫
				return RedirectToAction("Index","Home");
			}
			return View();  //Contact.cshtml
		}

		//Action參數直接接收表單欄位資料()
		//public IActionResult Contact(string Name, string Email, string Phone)
		//{
		//	if (ModelState.IsValid) //檢查表單欄位資料，通過Server端驗證
		//	{
		//		//TODO: 將聯絡人資料存入資料庫
		//		return RedirectToAction("Index", "Home");
		//	}
		//	return View();  //Contact.cshtml
		//}

		//IFormCollection接收表單欄位資料,用Keys值取得欄位值(問卷和線上測驗)
		//public IActionResult Contact(IFormCollection coll)
		//{
		//	if (ModelState.IsValid) //檢查表單欄位資料，通過Server端驗證
		//	{
		//		//TODO: 將聯絡人資料存入資料庫
		//		return RedirectToAction("Index", "Home");
		//	}
		//	return View();  //Contact.cshtml
		//}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
