using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UDaspspice.Data;
using UDaspspice.Models;
using UDaspspice.Models.ViewModels;

namespace UDaspspice.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db { get; set; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;

        }

        public async Task<IActionResult> Index()
        {
            CustomersIndexViewModel IndexViewModel = new CustomersIndexViewModel()
            {
                MenuItems = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Categories = await _db.Category.ToListAsync(),
                Coupons = await _db.Coupon.Where(m => m.IsActive == true).ToListAsync()

            };
            return View(IndexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
