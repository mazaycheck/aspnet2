using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDaspspice.Data;
using UDaspspice.Models;
using UDaspspice.Models.ViewModels;

namespace UDaspspice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        ApplicationDbContext _db { get; set; }

        [TempData]
        string StatusMessage { get; set; }

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Subcategory.Include(m => m.Category).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = new SubCategory(),
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryAndSubCategoryViewModel model)
        {
 
            if (ModelState.IsValid)
            {
              
                var DoesExistingEntries = await _db.Subcategory.Include(m => m.Category).Where(m => m.Name == model.Subcategory.Name && m.Category.Id == model.Subcategory.CategoryId).ToListAsync();
                if(DoesExistingEntries.Count > 0) 
                {
                    StatusMessage = "Error: please use another sub-category name!";
                }
                else
                {
                    _db.Subcategory.Add(model.Subcategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                
            }
      
            CategoryAndSubCategoryViewModel model2 = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = new SubCategory(),
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
                Message = StatusMessage
            };
            return View(model2);
           
            
        }

        [ActionName("GetSubCategoryList")]
        public async Task<IActionResult> GetSubCategoryList(int id)
        {
            List<SubCategory> categories = new List<SubCategory>();
            categories = await (from SubCategory in _db.Subcategory
                          where SubCategory.CategoryId == id
                          select SubCategory).ToListAsync();
            var data =  Json(new SelectList(categories, "Id", "Name"));
            
            return data;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var subcategoryfromdb = await _db.Subcategory.SingleOrDefaultAsync(m => m.Id == id);

            if(subcategoryfromdb == null)
            {
                return NotFound();
            }

            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = subcategoryfromdb,
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryAndSubCategoryViewModel model)
        {
            var subcategoryfromdb = await _db.Subcategory.SingleOrDefaultAsync(m => m.Id == id);
            if (ModelState.IsValid)
            {

                var DoesExistingEntries = await _db.Subcategory.Include(m => m.Category).Where(m => m.Name == model.Subcategory.Name && m.Category.Id == id).ToListAsync();
                if (DoesExistingEntries.Count > 0)
                {
                    StatusMessage = "Error: please use another sub-category name!";
                }
                else
                {
              
                    subcategoryfromdb.Name = model.Subcategory.Name;
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }

            }
     
            CategoryAndSubCategoryViewModel model2 = new CategoryAndSubCategoryViewModel()
            {
                
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = subcategoryfromdb,
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
                Message = StatusMessage
            };
            return View(model2);


        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var subcategoryfromdb = await _db.Subcategory.SingleOrDefaultAsync(m=>m.Id == id);
            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = subcategoryfromdb,
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
                Message = StatusMessage
            };
            return View(model);

        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var subcategoryfromdb = await _db.Subcategory.SingleOrDefaultAsync(m => m.Id == id);
            CategoryAndSubCategoryViewModel model = new CategoryAndSubCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Subcategory = subcategoryfromdb,
                SubcategoryList = await _db.Subcategory.OrderBy(m => m.Name).Select(m => m.Name).ToListAsync(),
                Message = StatusMessage
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return NotFound();
            var subcategoryfromdb = await _db.Subcategory.SingleOrDefaultAsync(m => m.Id == id);
            _db.Subcategory.Remove(subcategoryfromdb);
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}