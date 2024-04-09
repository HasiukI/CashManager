using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CashManager.ViewModel
{
    internal class CategoryViewModel : ViewModelBase
    {
        private readonly string[] _colorsAtributteCategories={ "#FF34AC25", "#FFC81A1A" };
        private readonly string[] _namesAtributteCategories={"Profit","Costs"};


        public CategoryViewModel() { 
            NameAttributeMenu = _namesAtributteCategories[0];
            ColorAttributeMenu = _colorsAtributteCategories[0];
        }




        //Property Model
        private string _nameCategory;
        public string NameCategory { get => _nameCategory; set => _nameCategory = value; }

        private int _priceCategory;
        public int PriceCategory { get => _priceCategory; set => _priceCategory = value; }

        private bool _isCosts;

        private string _colorCategory;
        public string ColorCategory { get => _colorCategory; set => _colorCategory = value; }

        private BitmapImage _imageCategory;
        public BitmapImage ImageCategory { get => _imageCategory; set => _imageCategory = value; }


        //Design
        public string ColorAttributeMenu { get; private set; }
        public string NameAttributeMenu { get; private set; }

        public ICommand ChangeCategory
        {
            get => new RelayCommand(() =>
            {
                if(NameAttributeMenu == _namesAtributteCategories[0])
                {
                    NameAttributeMenu = _namesAtributteCategories[1];
                    ColorAttributeMenu = _colorsAtributteCategories[1];
                }
                else
                {
                    NameAttributeMenu = _namesAtributteCategories[0];
                    ColorAttributeMenu = _colorsAtributteCategories[0];
                }

                onPropertyChanged(nameof(ColorAttributeMenu));
                onPropertyChanged(nameof(NameAttributeMenu));
            });
        }
    }
}
