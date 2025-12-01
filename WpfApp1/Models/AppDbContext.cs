using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsAccounting> GoodsAccountings { get; set; }
        public DbSet<OrderEmployee> OrderEmployee { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrdersGoods> OrdersGoods { get; set; }
        public DbSet<Postlist> Postlist { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet <SuppliersGoods> SuppliersGoods { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL
                (
                    @"Server=(remotedb)\mysqlremotedb;tompsons_stud21;Trusted_Connection=True;"
                );
        }
    }
}
