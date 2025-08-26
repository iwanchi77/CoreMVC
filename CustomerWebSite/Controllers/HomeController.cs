using CustomerWebSite.Models;
using CustomerWebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace CustomerWebSite.Controllers
{

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		NorthwindContext _context;
		IMemoryCache _cache;

		public HomeController(ILogger<HomeController> logger, NorthwindContext context, IMemoryCache cache)
		{
			_logger = logger;
			_context = context;
			_cache = cache;
		}

		public IActionResult Index()
		{
			ViewBag.CustomerCountry = new SelectList(_context.Customers.Select(c=> c.Country).Distinct());
			//等同於(舊名稱)
			//ViewData["CustomerCountry"] = new SelectList(_context.Customers.Select(c => c.Country).Distinct()); 
			ViewBag.Script = $"alert('客戶人數:{_context.Customers.Count()}')";

			//設定 Session，並設定 Session 的值
			HttpContext.Session.SetString("SessionKey", "SessionValue");

			//設定快取 Cache
			MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions();
			cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromDays(1));
			cacheEntryOptions.SetPriority(CacheItemPriority.Normal);
			_cache.Set<string>("CacheKey", "CacheValue");

			//設定 Cookie
			CookieOptions cookieOptions = new CookieOptions();
			cookieOptions.Expires = DateTime.Now.AddYears(30);
			cookieOptions.HttpOnly = true;
			cookieOptions.Secure = true;
			Response.Cookies.Append("CookieKey", "CookieValue",cookieOptions);


			return View();  //Index.cshtml
		}

		public IActionResult Privacy()
		{
			//讀取 Session 的值
			string? SessionValue = HttpContext.Session.GetString("SessionKey");
			if (SessionValue != null)
			{

			}
			//讀取快取 Cache 的值
			string? CacheValue = _cache.Get<string>("CacheKey");
			if(CacheValue != null)
			{
				
			}

			//讀取 Cookie 的值
			string? CookieValue = Request.Cookies["CookieKey"];
			if(CookieValue != null)
			{

			}

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
		public IActionResult Contact([Bind("Name,Email,Phone")]ContactViewModel cvm)//明列所有可以接收的欄位
		{
			if (ModelState.IsValid) //檢查表單欄位資料，通過Server端驗證
			{
				TempData["Message"] = "成功!";
				//TODO: 將聯絡人資料存入資料庫
				return RedirectToAction("Index", "Home");
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
