using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.ViewModel
{
    class MainViewModel
    {
        public CategoryViewModel Category { get; set; }
        public CashViewModel Cash { get; set; }

        public MainViewModel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            Category = new CategoryViewModel(connectionString);
            Cash = new CashViewModel(connectionString);
        }
    }
}
