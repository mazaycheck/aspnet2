using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Models.ViewModels
{
    public class MenuViewModel
    {
        public MenuItem MenuItem { get; set; }
        public IEnumerable<Category> CategoryEnumerable { get; set; }
        public IEnumerable<SubCategory> SubCategoryEnumerable { get; set; }

    }
}
