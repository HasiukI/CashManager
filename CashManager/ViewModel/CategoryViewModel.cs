using CashManager.Repository;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace CashManager.ViewModel
{
    internal class CategoryViewModel : ViewModelBase
    {
        private readonly string[] _colorsAtributteCategories={ "#FF34AC25", "#FFC81A1A" };
        private readonly string[] _namesAtributteCategories={"Profit","Costs"};

        private readonly IRepository _repository = null;

        public CategoryViewModel() { 
            NameAttributeMenu = _namesAtributteCategories[0];
            ColorAttributeMenu = _colorsAtributteCategories[0];

            ColorsForCategory = new List<string>();
            ColorsForCategory.Add("#39878c");
            ColorsForCategory.Add("#fe4505");
            ColorsForCategory.Add("#ffc117");
            ColorsForCategory.Add("#ffc117");
            ColorsForCategory.Add("#ffc117");
            ColorsForCategory.Add("#ffc117");
            ColorsForCategory.Add("#ffc117");
            ColorsForCategory.Add("#ffc117");

            _repository = new FileRepository();
            ImagesForCategory = _repository.ReadAllStaticPictures();
        }



        //static File
        public List<string> ColorsForCategory { get; }
        public List<BitmapImage> ImagesForCategory { get; }

        //Property Model
        private string _nameNewCategory;
        public string NameNewCategory { get => _nameNewCategory;
            set { 
                if(Regex.IsMatch(value, @"[№;%*?:?;!]"))
                {
                    ExeptionNameCategory = "Не можна використовувати спеціальні символи";
                    onPropertyChanged(nameof(ExeptionNameCategory));
                }
                if (value.Length > 20)
                {
                    ExeptionNameCategory = "Задовга назва";
                    onPropertyChanged(nameof(ExeptionNameCategory));
                }
            }
        }
        public string ExeptionNameCategory { get; private set; }

        private int _priceNewCategory;
        public int PriceNewCategory { get => _priceNewCategory; set => _priceNewCategory = value; }
        public string ExaptionPriceCategory { get; private set; }

        private bool _isCosts;

        private string _colorNewCategory;
        public string ColorNewCategory { get => _colorNewCategory; set => _colorNewCategory = value; }

        private BitmapImage _imageNewCategory;
        public BitmapImage ImageNewCategory { get => _imageNewCategory; set => _imageNewCategory = value; }


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
