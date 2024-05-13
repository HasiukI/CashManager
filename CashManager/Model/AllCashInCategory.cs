using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Model
{
    internal class AllCashInCategory
    {
        public Category Category { get; set; }
        public ObservableCollection<Cash> Cashes { get; set; }
        public decimal TotalSum { get; set; }
    }
}
