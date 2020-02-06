using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDaspspice.Data;
using UDaspspice.Models;
using UDaspspice.Models.ViewModels;

namespace UDaspspice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostenv;

        [BindProperty]
        public MenuViewModel MenuVM { get; set; }
        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment hostenv)
        {
            _db = db;
            _hostenv = hostenv;
            MenuVM = new MenuViewModel()
            {
                CategoryEnumerable = _db.Category,
                MenuItem = new MenuItem()
            };
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).OrderBy(m => m.Name).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(MenuVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
 
     
            if (!ModelState.IsValid) return View(MenuVM);
            else
            {
                _db.MenuItem.Add(MenuVM.MenuItem);
                await _db.SaveChangesAsync();
            }

            var webroot = _hostenv.WebRootPath;
            var files = Request.Form.Files;

      
            var menuItemfromDb = await _db.MenuItem.FindAsync(MenuVM.MenuItem.Id);

            var uploads = Path.Combine(webroot, "images");

            string filename;

            if(files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                filename = MenuVM.MenuItem.Id + extension;
                var path = Path.Combine(uploads, filename);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }                
            }
            else
            {
                filename = MenuVM.MenuItem.Id + ".png";
                var defaultfilename = "MenuItemDefault.png";                
                var defaultpath = Path.Combine(uploads, defaultfilename);
                var newpath = Path.Combine(uploads, filename);
                System.IO.File.Copy(defaultpath, newpath);
            }

            menuItemfromDb.Image = @"\images\" + filename;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            else
            {
                MenuVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).FirstAsync(m => m.Id == id);
                MenuVM.SubCategoryEnumerable = await _db.Subcategory.Where(m => m.Category.Id == MenuVM.MenuItem.CategoryId).ToListAsync();
            }
            return View(MenuVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {

            var menuItemFromDb = await _db.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid) return View(MenuVM);
            else
            {                
                menuItemFromDb.Name = MenuVM.MenuItem.Name;
                menuItemFromDb.Price = MenuVM.MenuItem.Price;
    
                menuItemFromDb.Description = MenuVM.MenuItem.Description;
                menuItemFromDb.Espice = MenuVM.MenuItem.Espice;
                menuItemFromDb.SubCategoryId = MenuVM.MenuItem.SubCategoryId;
                _db.MenuItem.Update(menuItemFromDb);
                await _db.SaveChangesAsync();
            }

            var webroot = _hostenv.WebRootPath;
            var files = Request.Form.Files;            
            var uploads = Path.Combine(webroot, "images");
            string filename;

            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                filename = MenuVM.MenuItem.Id + extension;
                var path = Path.Combine(uploads, filename);

                var oldfilename = menuItemFromDb.Image.TrimStart('\\');
                var oldpath = Path.Combine(uploads, oldfilename);

                if (System.IO.File.Exists(oldpath))
                {
                    System.IO.File.Delete(oldpath);
                }

                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                menuItemFromDb.Image = @"\images\" + filename;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            else
            {
                MenuVM.MenuItem = await _db.MenuItem.SingleOrDefaultAsync(m=>m.Id == id);
                MenuVM.CategoryEnumerable = await _db.Category.ToListAsync();
            }
            return View(MenuVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            else
            {
                MenuVM.MenuItem = await _db.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
                MenuVM.CategoryEnumerable = await _db.Category.ToListAsync();
            }
            return View(MenuVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null) return NotFound();
            else
            {
                var MenuItemFromDb = await _db.MenuItem.SingleOrDefaultAsync(m => m.Id == id);
                if(MenuItemFromDb.Image != null) { 
                    var webroot = _hostenv.WebRootPath;                              
                    var oldfilename = MenuItemFromDb.Image.TrimStart('\\');
                    var oldpath = Path.Combine(webroot, oldfilename);

                    if (System.IO.File.Exists(oldpath))
                    {
                        System.IO.File.Delete(oldpath);
                    }
                }
                _db.MenuItem.Remove(MenuItemFromDb);
                await _db.SaveChangesAsync();


            }
            return RedirectToAction("Index");
        }

    }
}