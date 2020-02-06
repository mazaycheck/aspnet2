using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UDaspspice.Models;

namespace UDaspspice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> Subcategory { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
    }
}
