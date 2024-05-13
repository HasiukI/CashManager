using CashManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Model
{
    internal class HistoryDay :ViewModelBase
    {

        public HistoryDay() {
            Date = DateTime.Now;
            HistoriesInDay = null;

        }
        public DateTime Date { get; set; }

        private decimal _totalProfit;
        public decimal TotalProfit { 
            get=>_totalProfit; 
            set {
                if (value != _totalProfit)
                {
                    _totalProfit = value;
                    onPropertyChanged(nameof(TotalProfit));
                }
            } }

        private decimal _totalCosts;
        public decimal TotalCosts
        {
            get => _totalCosts;
            set
            {
                if (value != _totalCosts)
                {
                    _totalCosts = value;
                    onPropertyChanged(nameof(TotalCosts));
                }
            }
        }

        public ObservableCollection<AllInfoForCash> HistoriesInDay { get; set; }

    }
}
