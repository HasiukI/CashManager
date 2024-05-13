using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class MainViewModel 
    {
        public CategoryViewModel Category { get; set; }
        public PropertyViewModel Property { get; set; }
        public CashViewModel Cash { get; set; }
        public HistoryViewModel History { get; set; }
        public Data Data { get; set; }
        public LanguagesViewModel Language { get; set; }

        private IRepository _repository;

        public MainViewModel() {
            _repository = new FileRepository();
            Language = new LanguagesViewModel(_repository.GetMainInfo().Language);
            Data = new Data(Language,_repository);
            

            History = new HistoryViewModel(Data, _repository, Language);
            Category = new CategoryViewModel(Data,_repository, Language);
            Cash = new CashViewModel(Data, _repository,Language);
            Property = new PropertyViewModel(Data,_repository);

            ChangeLanguageToENG = new Command(ChangeEng);
            ChangeLanguageToUKR = new Command(ChangeUkr);
        }

        
        public ICommand ChangeLanguageToENG { get; } 
        public ICommand ChangeLanguageToUKR { get; } 

        private void ChangeEng() {
            Language.CurentNameLanguage = "Eng";
            History.UpdateViewInfo();
            Data.UpdateView();
        }

        private void ChangeUkr() {
            Language.CurentNameLanguage = "Ukr";
            History.UpdateViewInfo();
            Data.UpdateView();
        }
    }
}
