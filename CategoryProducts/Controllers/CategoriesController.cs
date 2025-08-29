using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CategoryProducts.Models;

namespace CategoryProducts.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index() 
		{
			//不載入圖片資料
			return View(_context.Categories.Select(c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = null //不載入圖片資料，節省記憶體
			}));
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id) 
		{
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.Select(c => new Category 
            {   
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture =null, //不載入圖片資料，節省記憶體

			}).FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

		//GET: Categories/GetPicture/1
		[HttpGet]
        public async Task<FileResult> GetPicture(int id) //傳回圖片
		{
            Category? c = await _context.Categories.FindAsync(id);
            byte[]? ImageData = c?.Picture;//c有值就用c.Picture,沒有值就用預設值
		    return File(ImageData, "image/bmp");//建成檔案回傳
		}

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] Category category)
        {
            if (ModelState.IsValid)
            {
				if (Request.Form.Files["Picture"] != null)
				{
					using (BinaryReader reader = new BinaryReader(Request.Form.Files["Picture"].OpenReadStream()))
					{
						category.Picture = reader.ReadBytes((int)Request.Form.Files["Picture"].Length);
					}
				}
				_context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
			{
                return NotFound();
            }

            var category = await _context.Categories.Select(c=>new Category 
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = null //不載入圖片資料，節省記憶體

			}).FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit =2048000)]
        [RequestSizeLimit(2048000)]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Category? c = await _context.Categories.FindAsync(category.CategoryId);
                if (Request.Form.Files["Picture"] != null)
                {
                    using (BinaryReader reader = new BinaryReader(Request.Form.Files["Picture"].OpenReadStream()))
                    {
                        category.Picture = reader.ReadBytes((int)Request.Form.Files["Picture"].Length);
                    }
                }
                else 
                {
                    category.Picture = c.Picture; //沒有上傳圖片就用原來的圖片
				}
                _context.Entry(c).State = EntityState.Detached; //將c物件從EF核心的追蹤清單中移除
				try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
			{
                return NotFound();
            }

            var category = await _context.Categories.Select(c => new Category 
            { 
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = null //不載入圖片資料，節省記憶體

			}).FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
