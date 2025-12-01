using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    internal class MainViewModel
    {
        public ObservableCollection<Category> Category { get; set; }
        public ObservableCollection<Delivery> Deliveries { get; set; }
        public ObservableCollection<Goods> Goods { get; set; }
        public ObservableCollection<GoodsAccounting> GoodsAccounting { get; set; }
        public ObservableCollection<OrderEmployee> OrderEmployees { get; set; }
        public ObservableCollection<Orders> Orders { get; set; }
        public ObservableCollection<OrdersGoods> OrdersGoods { get; set; }
        public ObservableCollection<Postlist> Postlist { get; set; }
        public ObservableCollection<Role> Roles { get; set; }
        public ObservableCollection <Suppliers> Suppliers { get; set; }
        public ObservableCollection<SuppliersGoods> SuppliersGoods { get; set; }
        public ObservableCollection<UserPost> UserPost { get; set; }
        public ObservableCollection<Users> Users { get; set; }
        public ObservableCollection<Warehouse> Warehouse { get; set; }

        public MainViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                Users = new ObservableCollection<Users>(context.Users.Include(e => e.RoleId).ToList());
                Category = new ObservableCollection<Category>(context.Categories.ToList());
                Deliveries = new ObservableCollection<Delivery>(context.Deliveries.ToList());
                Goods = new ObservableCollection<Goods>(context.Goods.Include(e => e.CategoryId).ToList());
                GoodsAccounting = new ObservableCollection<GoodsAccounting>(context.GoodsAccountings.Include(e => e.GoodsID).ToList());
                OrderEmployees = new ObservableCollection<OrderEmployee>(context.OrderEmployee.Include(e => e.OrderID).ToList());
                Orders = new ObservableCollection<Orders>(context.Orders.Include(e => e.Id).ToList());
                OrdersGoods = new ObservableCollection<OrdersGoods>(context.OrdersGoods.Include(e => e.Id).ToList());
                Postlist = new ObservableCollection<Postlist>(context.Postlist.ToList());
                Roles = new ObservableCollection<Role>(context.Roles.ToList());
                Suppliers = new ObservableCollection<Suppliers>(context.Suppliers.ToList());
                SuppliersGoods = new ObservableCollection<SuppliersGoods>(context.SuppliersGoods.Include(e => e.SupplierID).ToList());
                UserPost = new ObservableCollection<UserPost>(context.UserPosts.Include(e => e.UserID).ToList());
                Warehouse = new ObservableCollection<Warehouse>(context.Warehouse.ToList());
            }
        }
    }
}
