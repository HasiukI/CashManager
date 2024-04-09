using CashManager.Pages;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    class PagesViewModel : ViewModelBase
    {
        private CalendarPage calendarPage;
        private MainPage mainPage;

        public PagesViewModel()
        {
            calendarPage = new CalendarPage();
            mainPage = new MainPage();
            _curentPage = mainPage;
        }


        private Page _curentPage;
        public Page CurentPage { get => _curentPage; }

        public ICommand ShowMain 
        {
            get => new RelayCommand(() => { _curentPage = mainPage; onPropertyChanged(nameof(CurentPage)); });
        }

        public ICommand ShowCalendar
        {
            get => new RelayCommand(() => { _curentPage = calendarPage; onPropertyChanged(nameof(CurentPage)); });
        }

    }
}
