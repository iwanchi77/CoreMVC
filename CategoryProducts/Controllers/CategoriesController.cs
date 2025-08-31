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
        public async Task<IActionResult> Index() //因GetPicture方法會傳回圖片檔案，所以Index方法要做不載入圖片資料
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
			{   //因GetPicture方法會傳回圖片檔案，所以Details方法要做不載入圖片資料
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
        public async Task<FileResult> GetPicture(int id) //async非同步方法，傳回圖片檔案
		{
            Category? c = await _context.Categories.FindAsync(id);  //尋找主鍵值為id的資料
			byte[]? ImageData = c?.Picture;  //因為Pictrue有可能會沒有值，所以要用c?.Picture，避免NullReferenceException
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
					ReadUploadImage(category);
				}
				_context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

		private void ReadUploadImage(Category category)
		{
			using (BinaryReader reader = new BinaryReader(Request.Form.Files["Picture"].OpenReadStream()))
			{
				category.Picture = reader.ReadBytes((int)Request.Form.Files["Picture"].Length);
			}
		}

		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
			{
                return NotFound();
            }

			//var category = await _context.Categories.FindAsync(id); //原本的寫法，會載入圖片資料
			var category = await _context.Categories.Select(c=>new Category
			{//因GetPicture方法會傳回圖片檔案，所以Edit方法要做不載入圖片資料
				CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Picture = null //不載入圖片資料，節省記憶體

			}).FirstOrDefaultAsync(m => m.CategoryId == id); //新的寫法，不載入圖片資料，FistOrDefaultAsync會傳回Category物件或null
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
                if (Request.Form.Files["Picture"] != null) //!= null表示有上傳圖片
				{
					ReadUploadImage(category);
				}
                else 
                {
                    category.Picture = c.Picture; //沒有上傳圖片就用原來的圖片
				}
                _context.Entry(c).State = EntityState.Detached; //將c物件從EF核心的追蹤清單中移除，避免更新時發生衝突。卸離c，讓EF核心不再追蹤它。
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
			{ //因GetPicture方法會傳回圖片檔案，所以Delete方法要做不載入圖片資料
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
