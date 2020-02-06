using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDaspspice.Data;
using UDaspspice.Models;

namespace UDaspspice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        

        private ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {            
            return View(await _db.Category.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                var cat = await _db.Category.FindAsync(id);
                return View(cat);
            }        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var cat = await _db.Category.FindAsync(id);
                return View(cat);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                var cat = await _db.Category.FindAsync(id);
                _db.Category.Remove(cat);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var cat = await _db.Category.FindAsync(id);
                return View(cat);
            }
        }



    }
}