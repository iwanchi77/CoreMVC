using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
	public class FuenController : Controller
	{
		//IActionResult: Action動作回傳型別
		//Controller: 控制器基底類別，提供Action動作方法的功能
		public IActionResult Index() //Action動作函式
		{
			return View(); //Index.cshtml 會顯示Fuen/Index的內容
		}
	}
}
