using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.ViewModel
{
    internal class MainViewModel
    {
        public CategoryViewModel Category { get; set; }

        public MainViewModel() { 
            Category = new CategoryViewModel(); 
        }
    }
}
