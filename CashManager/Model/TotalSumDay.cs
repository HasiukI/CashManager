using CashManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Model
{
    internal class TotalSumDay : ViewModelBase
    {
        public decimal _totalProfits;
        public decimal TotalProfit
        {
            get { return _totalProfits; }
            set
            {
                if (_totalProfits != value)
                {
                    _totalProfits = value;
                    onPropertyChanged(nameof(TotalProfit));
                }
            }
        }

        private decimal _totalCosts;
        public decimal TotalCosts {
            get { return _totalCosts; }
            set
            {
                if (_totalCosts != value)
                {
                    _totalCosts = value;
                    onPropertyChanged(nameof(TotalCosts));
                }
            }
        }
        public DateTime CurentDate { get; set; }
    }
}

