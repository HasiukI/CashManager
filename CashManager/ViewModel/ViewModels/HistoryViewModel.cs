using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.ViewModel.ViewModels
{
    internal class HistoryViewModel :ViewModelBase
    {
        private readonly IRepository _repository =null;

        public ObservableCollection<History> Histories { get; private set; }

        public HistoryViewModel(string connectionString)
        {
            _repository = new DapperRepository(connectionString);

            Histories = new ObservableCollection<History>(_repository.LoadHistory(DateTime.UtcNow));
        }

       

    }
}
