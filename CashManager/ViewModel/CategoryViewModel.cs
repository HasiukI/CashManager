using CashManager.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class CategoryViewModel :ViewModelBase
    {
        public CategoryViewModel() { 
           
           
            List<Category> categories = new List<Category>();

            categories.Add(new Category() { Id = 1, Name = "ct1", Price = 100 });
            categories.Add(new Category() { Id = 1, Name = "ct2", Price = 100 });

            _categories = new ObservableCollection<Category>(categories);
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
