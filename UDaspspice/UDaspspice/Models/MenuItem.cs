using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public enum Spicyness { EASY = 1, MEDIUM = 2, HOT = 3};

        public string Espice { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual  Category Category { get; set; }

        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual  SubCategory SubCategory { get; set; }

    }
}
