using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class CashViewModel : ViewModelBase
    {
        private readonly DapperRepository _repository = null;


        public ICommand CreateCashItem { get; private set; }
        public ICommand CountUpCash {  get; private set; } 
        public ICommand CountDownCash {  get; private set; } 

        public CashViewModel(string connectionString)
        {
            _repository = new DapperRepository(connectionString);

            CreateCashItem = new AsyncCommand(CreateCash);
            CountUpCash = new Command(CountUp);
            CountDownCash = new Command(CountDown);

            CountCash = 1;
        }

        private void CountUp()
        {
            TotalCash = (int)_curentCategory.Price * ++CountCash;
            onPropertyChanged(nameof(TotalCash));
            onPropertyChanged(nameof(CountCash));
        }

        private void CountDown()
        {
            if (CountCash > 1)
            {
                TotalCash = (int)_curentCategory.Price * (--CountCash);
                onPropertyChanged(nameof(TotalCash));
                onPropertyChanged(nameof(CountCash));
            }
        }



        private async Task CreateCash()
        {
            Cash cash = new Cash() { Price = (int)TotalCash, CategoryId=_curentCategory.Id, Count=CountCash, CreatedAt=DateTime.UtcNow};
            await _repository.CreateCash(cash);
        }



        public int CountCash { get; private set; }

        private Category _curentCategory;
        public Category CurentCategory
        {
            get => _curentCategory;
            set
            {
                _curentCategory = value;
                TotalCash = _curentCategory.Price;
                onPropertyChanged(nameof(TotalCash));
                onPropertyChanged(nameof(CurentCategory));
            }
        }


       public decimal TotalCash { get; private set; }
    }
}
