using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class HistoryViewModel : ViewModelBase
    {
        private readonly Data _data; 
        private readonly IRepository _repository = null;
        private readonly LanguagesViewModel _languages;

        public ICommand NextMounthCommand { get; }
        public ICommand PrevMounthCommand { get; }

        public HistoryViewModel(Data data, IRepository repository, LanguagesViewModel languages)
        {
            this._data = data;
            this._repository = repository;
            this._languages= languages;

            
            SelectMounth = this._data.SelectedDate;

            var history = _repository.GetAllHistoryInMounth(SelectMounth, this._data.AllCategories);
            this._data.DaysHistory = new ObservableCollection<HistoryDay>(history);
            this._data.SelectedDayHistory = history.Where(h => h!=null && h.Date.Date == SelectMounth.Date.Date).FirstOrDefault();

            NextMounthCommand = new Command(NextMounth);
            PrevMounthCommand = new Command(PrevMounth);
        }

        #region Property
        public DateTime SelectMounth { get; private set; }

        public string NameMounthForMain { get => _languages.GetMaounthName(SelectMounth.Month, false); }
        public string NameMounthForCalendaer { get => _languages.GetMaounthName(SelectMounth.Month, true); }
        #endregion

        #region Command
        public void NextMounth()
        {
            SelectMounth = SelectMounth.AddMonths(1);
            UpdateHistoryBySelectMount();
        }

        public void PrevMounth()
        {
            SelectMounth = SelectMounth.AddMonths(-1);
            UpdateHistoryBySelectMount();
        }
        #endregion

        private void UpdateHistoryBySelectMount()
        {
            this._data.DaysHistory.Clear();
            foreach (HistoryDay day in _repository.GetAllHistoryInMounth(SelectMounth, this._data.AllCategories))
            {
                this._data.DaysHistory.Add(day);
            }

            this._data.SelectedDate = SelectMounth;
            this._data.UpdateDiagram(SelectMounth);
            onPropertyChanged(nameof(NameMounthForMain));
            onPropertyChanged(nameof(NameMounthForCalendaer));
        }
        
        public void UpdateViewInfo()
        {
            onPropertyChanged(nameof(NameMounthForMain));
            onPropertyChanged(nameof(NameMounthForCalendaer));
        }

    }
}
