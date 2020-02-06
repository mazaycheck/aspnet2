using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Models.ViewModels
{
    public class CustomersIndexViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
