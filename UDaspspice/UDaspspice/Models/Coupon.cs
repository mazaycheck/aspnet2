using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UDaspspice.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public enum Etype { percent = 0, dollar = 1}
        [Required]
        public int Amount { get; set; }
        [Required]
        public  int MinimumSum { get; set; }
        public bool IsActive { get; set; }
        public byte[] Image { get; set; }
    }
}
