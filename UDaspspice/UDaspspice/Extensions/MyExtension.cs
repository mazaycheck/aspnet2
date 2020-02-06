using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Extensions
{
    public static class MyExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items                   
                   select new SelectListItem
                   {
                       Text = GetPropertyValue(item, "Name"),
                       Value = GetPropertyValue(item, "Id"),
                       Selected = GetPropertyValue(item, "Id").Equals(selectedValue.ToString()),
                   };
        }

        public static string GetPropertyValue<T>(T item, string propertyname) 
        {
            return item.GetType().GetProperty(propertyname).GetValue(item, null).ToString();
        }
    }
}
