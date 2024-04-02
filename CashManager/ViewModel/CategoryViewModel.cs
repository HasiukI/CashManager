using CashManager.Model;
using CashManager.Repository;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers.Commands;
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
        public ObservableCollection<Category> Categories { get; }

        // Comands
        public ICommand CreateNewCategory { get; private set; }

        public CategoryViewModel() {

            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            _repository = new DapperRepository(connectionString);

            Categories = new ObservableCollection<Category>(_repository.GetAllCategory());


            CreateNewCategory = new AsyncCommand(CreateCategory);
        }
       
        private async Task<bool> CreateCategory()
        {

            if (_nameCategory.Length == 0)
            {
                return false;
            }

            Category category = new Category() { isCosts = IsCostsCategory, Name=_nameCategory, Price=_priceCategory };

            category.Id = await _repository.CreateCategoryAsync(category);

            Categories.Add(category);

            return true;
        }

        private string _nameCategory;
        public string NameCategory { get => _nameCategory; set => _nameCategory = value; }

        private int _priceCategory;
        public int PriceCategory { get=> _priceCategory; set => _priceCategory = value; }

        private bool _isCosts;
        public bool IsCostsCategory { get => _isCosts; set => _isCosts = value; }


        private Category _curentCategory;

        public Category CurentCategory {
            get => _curentCategory; 
            set { 
                  _curentCategory = value;
                onPropertyChanged(nameof(CurentCategory));
            } 
        }
        //public ICommand CreateCategory
        //{
        //    get => new RelayCommand(() => Categories.Add(new Category() { Id = 1, Name = _nameCategory, Price = _priceCategory }));
        //}

    }
}
