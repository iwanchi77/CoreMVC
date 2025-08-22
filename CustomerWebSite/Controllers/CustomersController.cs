using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomerWebSite.Models;

namespace CustomerWebSite.Controllers
{
    [Route("/Customers/{action=Index}/{CustomerID?}")]


    public class CustomersController : Controller //控制器類別，處理客戶相關的請求
	{
		private readonly NorthwindContext _context;

		public CustomersController(NorthwindContext context) //建構函式，注入資料庫上下文
		{
            _context = context; //將注入的NorthwindContext物件賦值給私有欄位_context
		}

        // GET: Customers
        //public IActionResult Index() 同步函式 傳回所有客戶資料的視圖

		public async Task<IActionResult> Index() //傳回所有客戶資料
		{
			//return View(await _context.Customers.ToListAsync());
			return View(_context.Customers); //Index.cshtml 生成客戶列表
		}

		// GET: Customers/Details/5
		public async Task<IActionResult> Details(string CustomerID) //傳回特定客戶資料
		{
            if (CustomerID == null)
            {
                return NotFound();      //傳回HTTP 404錯誤
			}

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == CustomerID);
            if (customer == null)
            {
                return NotFound();      //傳回HTTP 404錯誤
			}

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create() //傳回建立新客戶的表單
		{
            return View();            //Create.cshtml 生成空白表單
		}

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]                 //指定此方法只處理HTTP POST請求
		[ValidateAntiForgeryToken] //防止跨站請求偽造攻擊
		public async Task<IActionResult> Create([Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);                 //將新客戶加入資料庫上下文      
				await _context.SaveChangesAsync();      //將變更保存到資料庫
				return RedirectToAction(nameof(Index)); //重定向到Index動作，顯示所有客戶列表
			}
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string CustomerID) //把id參數傳入，傳回編輯特定客戶的表單
		{
            if (CustomerID == null)
            {
                return NotFound();  //傳回HTTP 404錯誤
			}

            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]                  //指定此方法只處理HTTP POST請求
		[ValidateAntiForgeryToken] //防止跨站請求偽造攻擊
		public async Task<IActionResult> Edit(string id, [Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)  //id參數用於識別要編輯的客戶
		{
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string CustomerID) //傳回刪除特定客戶的確認頁面
		{
            if (CustomerID == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == CustomerID);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]  //指定此方法處理HTTP POST請求，並將動作名稱設為"Delete"
		[ValidateAntiForgeryToken]        //防止跨站請求偽造攻擊
		public async Task<IActionResult> DeleteConfirmed(string CustomerID) //確認刪除特定客戶
		{
            var customer = await _context.Customers.FindAsync(CustomerID);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string CustomerID) //檢查特定客戶是否存在
		{
            return _context.Customers.Any(e => e.CustomerId == CustomerID);
        }
    }
}
