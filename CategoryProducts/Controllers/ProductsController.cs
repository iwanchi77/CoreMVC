using CategoryProducts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryProducts.Controllers
{
	public class ProductsController : Controller
	{
		private readonly NorthwindContext _context;

		public ProductsController(NorthwindContext context)
		{
			_context = context;
		}

		// GET: Products
		public async Task<IActionResult> Index()
		{
			return View();
		}

		// POST: Products/IndexJson
		[HttpPost]
		public async Task<IActionResult> IndexJson()
		{
			var draw = Request.Form["draw"].FirstOrDefault(); //draw參數原封不動回傳
			var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault()); //分頁起始筆數
			var length = Convert.ToInt32(Request.Form["length"].FirstOrDefault()); //分頁長度
			var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToLower(); //搜尋字串
			var query = _context.Products.AsQueryable(); //取得Products的IQueryable物件
			if (!string.IsNullOrEmpty(searchValue)) //如果searchValue有值
			{
				query = query.Where(p => p.ProductName.ToLower().Contains(searchValue));//選擇ProductName有包含searchValue的資料
			}
			var totalRecords = _context.Products.Count(); //未過濾的總筆數
			var filteredRecords = query.Count();          //過濾後的總筆數
			var data = query.Skip(start).Take(length); //Skip跳過前start筆資料,Take取length筆資料
			return Ok(new //回傳JSON資料
			{
				draw = draw, //draw參數原封不動回傳
				recordsTotal = totalRecords,  //未過濾的總筆數
				recordsFiltered = filteredRecords, //過濾後的總筆數
				data = data //分頁後的資料
			});
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
		{
			if (ModelState.IsValid)
			{
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
			return View(product);
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
		{
			if (id != product.ProductId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.ProductId))
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
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
			return View(product);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.ProductId == id);
		}
	}
}
