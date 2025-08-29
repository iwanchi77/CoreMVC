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
		[HttpGet] // 預設的 HTTP 動作方法是 GET，是預設可以不寫，方便看而已
		public IActionResult Index()
        {
             
			return View(); //Index.cshtml
		}

		// GET Home/Index
		// => uri 重複了，會錯誤
		//[HttpGet]
		//public IActionResult Index(int n)
		//{
		//	return View();  // 預設回傳 Views/Home/Index.cshtml
		//}

		// POST: Home/Index
		// => uri 包含動詞，所以這個跟上面的 Index 方法不會衝突
		[HttpPost] 
		public IActionResult Index(int x)
		{

			return View(); //Index.cshtml 會顯示Home/Index的POST結果
		}

		public IActionResult Privacy()
        {
			// 模擬一個例外（除以 0），實際執行時會進入錯誤處理流程
			//int x = 0;
			//int y = 10;
			//int z = y / x; // This will throw a DivideByZeroException

			return View(); //Privacy.cshtml 會顯示Home/Privacy的內容
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() // 預設回傳 Views/Shared/Error.cshtml，並傳入 ErrorViewModel
		{
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); //Error.cshtml 會顯示Home/Shared/Error的內容
		}
    }
}
