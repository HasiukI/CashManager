using CashManager.Model;
using CashManager.Repository;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class CategoryViewModel :ViewModelBase
    {
        private readonly DapperRepository _repository = null;

        public CategoryViewModel() {

            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            _repository = new DapperRepository(connectionString);
            _categories = new ObservableCollection<Category>( _repository.GetAllCategory().Result);
        }



        private ObservableCollection<Category> _categories { get; set; }
        public ObservableCollection<Category> Categories { get { return _categories; } }


        private string _nameCategory;
        public string NameCategory { get => _nameCategory; set => _nameCategory = value; }

        private int _priceCategory;
        public int PriceCategory { get=> _priceCategory; set => _priceCategory = value; }
       

        public ICommand CreateCategory
        {
            get => new RelayCommand(() => _categories.Add(new Category() { Id = 1, Name = _nameCategory, Price = _priceCategory }));
        }

    }
}
