using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDaspspice.Data;
using UDaspspice.Models;
using System.IO;
using System.Drawing;

namespace UDaspspice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {        
        public ApplicationDbContext _db;
        public CouponController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _db.Coupon.ToListAsync();
            return View(coupons);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }
            var files = HttpContext.Request.Form.Files;
            if(files.Count > 0)
            {
                byte[] pic1;
                using (var fs = files[0].OpenReadStream())
                {
                    using(var mem = new MemoryStream())
                    {
                        fs.CopyTo(mem);
                        pic1 = mem.ToArray();
                    }
                    
                }
                coupon.Image = pic1;
            }
            _db.Add(coupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var couponfromDb = await _db.Coupon.FirstOrDefaultAsync(m => m.Id == id);
            byte[] imagesbytes = couponfromDb.Image;

            return View(couponfromDb);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) { return NotFound(); }
            var coupon = await  _db.Coupon.FindAsync(id);
            return View(coupon);

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Coupon coupon)
        {
            _db.Coupon.Remove(coupon);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }
            var coupon = await _db.Coupon.FindAsync(id);
            return View(coupon);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCoupon(Coupon coupon)
        {
            var couponfromdb = await _db.Coupon.FindAsync(coupon.Id);
            couponfromdb.IsActive = coupon.IsActive;
            couponfromdb.MinimumSum = coupon.MinimumSum;
            couponfromdb.Type = coupon.Type;
            couponfromdb.Name = coupon.Name;
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {

                
                byte[] bfile;
                using(var filestream = files[0].OpenReadStream())
                {
                    using(var memorystream = new MemoryStream())
                    {
                        filestream.CopyTo(memorystream);
                        bfile = memorystream.ToArray();
                    }
                }
                couponfromdb.Image = bfile;
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}