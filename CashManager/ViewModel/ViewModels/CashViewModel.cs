using CashManager.Model;
using CashManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.ViewModel
{
    internal class CashViewModel : ViewModelBase
    {
        private readonly DapperRepository _repository = null;

        public CashViewModel(string connectionString)
        {
            _repository = new DapperRepository(connectionString);
        }


        private Category _curentCategory;

        public Category CurentCategory
        {
            get => _curentCategory;
            set
            {
                _curentCategory = value;
                onPropertyChanged(nameof(CurentCategory));
            }
        }
    }
}
