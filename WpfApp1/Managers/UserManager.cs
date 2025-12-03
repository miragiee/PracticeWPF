using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Managers
{
    internal class UserManager
    {
        public ObservableCollection<Users> users;

        public void AddUser()
        {
            users = new ObservableCollection<Users>();
        }

    }
}
