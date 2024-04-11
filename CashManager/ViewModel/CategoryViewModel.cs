using CashManager.Model;
using CashManager.Repository;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly List<Category> _allCategories;

        private readonly IRepository _repository = null;

        public ICommand CreateCategoryCommand { get;}

        public ObservableCollection<Category> Categories { get; private set; }


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

            CreateCategoryCommand = new AsyncCommand(CreateCategory);

            _allCategories = _repository.LoadCategories().ToList();

            Categories = new ObservableCollection<Category>(_allCategories.Where(c=> c.IsActual && !c.isCosts));
        }



        //static File
        public List<string> ColorsForCategory { get; }
        public List<BitmapImage> ImagesForCategory { get; }

        //Property Model
        private string _nameNewCategory;
        public string NameNewCategory { get => _nameNewCategory;
            set {

                bool isOk = true;

                if(Regex.IsMatch(value, @"[№;%*?:?;!]"))
                {
                    ExeptionNameCategory = "Не можна використовувати спеціальні символи";
                    onPropertyChanged(nameof(ExeptionNameCategory));
                    isOk = false;
                }
                if (value.Length > 20)
                {
                    ExeptionNameCategory = "Задовга назва";
                    onPropertyChanged(nameof(ExeptionNameCategory));
                    isOk = false;
                }

                if(isOk)
                {
                    _nameNewCategory = value;
                }
            }
        }
        public string ExeptionNameCategory { get; private set; }

        private int _priceNewCategory;
        public int PriceNewCategory { get => _priceNewCategory; set => _priceNewCategory = value; }
        public string ExaptionPriceCategory { get; private set; }

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
                    Categories.Clear();
                    foreach(var category in _allCategories.Where(c => c.IsActual && c.isCosts))
                    {
                        Categories.Add(category);
                    }
                   
                }
                else
                {
                    NameAttributeMenu = _namesAtributteCategories[0];
                    ColorAttributeMenu = _colorsAtributteCategories[0];
                    Categories.Clear();
                    foreach (var category in _allCategories.Where(c => c.IsActual && !c.isCosts))
                    {
                        Categories.Add(category);
                    }
                }

                onPropertyChanged(nameof(ColorAttributeMenu));
                onPropertyChanged(nameof(NameAttributeMenu));
            });
        }

        private async Task<bool> CreateCategory()
        {

            if (_nameNewCategory.Length == 0)
            {
                return false;
            }

            Category category = new Category() { 
                Name = _nameNewCategory,
                isCosts = (NameAttributeMenu == "Profit")? false : true,
                Price = _priceNewCategory, 
                Color = _colorNewCategory, 
                IsActual = true };

            if (_colorNewCategory == null)
            {
                category.Color = "#a23a96";
            }

            if (_imageNewCategory == null)
            {
                category.ImageName = "default.png";
                category.Image = new BitmapImage(new Uri(_repository.GetRootDirectoryImages() + "\\default.png", UriKind.Absolute));
            }
            else
            {
                string temp = _imageNewCategory.UriSource.ToString();
                category.ImageName = temp.Remove(0, temp.LastIndexOf("/") + 1);
                category.Image = _imageNewCategory;
            }


            await _repository.CreateCategoryAsync(category);

            return true;
        }

    }
}
