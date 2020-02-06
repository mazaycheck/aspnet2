using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Models.ViewModels
{
    public class CategoryAndSubCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public SubCategory Subcategory { get; set; }
        public List<string> SubcategoryList { get; set; }

        public string Message { get; set; }
    }
}
