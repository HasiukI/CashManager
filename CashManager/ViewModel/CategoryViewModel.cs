using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;


namespace CashManager.ViewModel
{
    internal class CategoryViewModel : ViewModelBase
    {
        private readonly Data _data = null;
        private readonly IRepository _repository = null;
        private LanguagesViewModel _language;

        //static File
        public List<string> ColorsForCategory { get; }
        public List<BitmapImage> ImagesForCategory { get; }
        //Design
        public string ColorAttributeMenu { get; private set; }
        public string NameAttributeMenu { get; private set; }
        //Command
        public ICommand CreateCategoryCommand { get;}
        public ICommand DeleteCategoryCommand { get;}
        public ICommand SetToUpdateCategoryCommand { get;}
        public ICommand UpdateCategoryCommand { get;}
        public ICommand ProfitNewCategory { get; }
        public ICommand CostsNewCategory { get; }
        public ICommand SetDefaultPropertyCommand { get; }
        public ICommand ShowPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }
  

        public CategoryViewModel(Data data, IRepository repository, LanguagesViewModel language) {
            this._data = data;
            this._repository = repository;
            this._language = language;

            NameAttributeMenu = _language.CategoryProfit;

            ColorsForCategory = SetColors(); 
            CreateCategoryCommand = new AsyncCommand(CreateCategory);
            DeleteCategoryCommand = new AsyncCommand(DeleteCategory);
            SetToUpdateCategoryCommand = new Command(SetToUpdate);
            UpdateCategoryCommand = new AsyncCommand(UpdateCategory);
            CostsNewCategory = new Command(TapCostsNewCategory);
            ProfitNewCategory = new Command(TapProfitNewCategory);
            SetDefaultPropertyCommand = new Command(SetDefaultProperty);
            ShowPopupCommand = new Command(ShowPopup);
            ClosePopupCommand = new Command(ClosePopup);
          
            ImagesForCategory = _repository.ReadAllStaticPictures();

        }

        #region Property

        public string ExeptionTypeCategory { get; private set; }
        public string ExeptionNameCategory { get; private set; }

        public bool? _isCostsNewCategory;

        private string _nameNewCategory;
        public string NameNewCategory
        {
            get => _nameNewCategory;
            set
            {

                if (_nameNewCategory != value)
                {
                    _nameNewCategory = value;
                    onPropertyChanged(nameof(NameNewCategory));
                }
            }
        }

        private decimal _priceNewCategory;
        public decimal PriceNewCategory
        {
            get => _priceNewCategory;
            set
            {
                if (value < 0)
                    value = 0;

                if (value != _priceNewCategory)
                {
                    _priceNewCategory = value;
                  
                }
                onPropertyChanged(nameof(PriceNewCategory));
            }
        }

        private bool _isShowAnimateClose;
        public bool IsShowAnimateClose
        { 
            get=> _isShowAnimateClose;
            set {
                if (value != _isShowAnimateClose)
                {
                    _isShowAnimateClose = value;
                    onPropertyChanged(nameof(IsShowAnimateClose));
                }
            } 
        }
    
        private string _colorNewCategory;
        public string ColorNewCategory
        {
            get => _colorNewCategory;
            set
            {
                if (_colorNewCategory != value)
                {
                    _colorNewCategory = value;
                    onPropertyChanged(nameof(ColorNewCategory));
                }
            }
        }

        private BitmapImage _imageNewCategory;
        public BitmapImage ImageNewCategory
        {
            get => _imageNewCategory;
            set
            {
                if (_imageNewCategory != value)
                {
                    _imageNewCategory = value;
                    onPropertyChanged(nameof(ImageNewCategory));
                }
            }
        }

        private double _widthNewCategoryProfit;
        public double WidthNewCategoryProfit { 
            get=>_widthNewCategoryProfit;
            set {
                if (value != _widthNewCategoryProfit)
                {
                    _widthNewCategoryProfit = value;
                    onPropertyChanged(nameof(WidthNewCategoryProfit));
                } 
            } 
        }
        private double _widthNewCategoryCosts;
        public double WidthNewCategoryCosts
        {
            get => _widthNewCategoryCosts;
            set
            {
                if (value != _widthNewCategoryCosts)
                {
                    _widthNewCategoryCosts = value;
                    onPropertyChanged(nameof(WidthNewCategoryCosts));
                }
            }
        }


        private bool _visiilityPrice; 
        public bool VisiilityPrice
        {
            get=> _visiilityPrice;
            set
            {
                if(value != _visiilityPrice)
                {
                    _visiilityPrice = value;
                    onPropertyChanged(nameof(VisiilityPrice));
                }
            }
        }

        private bool _visibilityCreateCategory;
        public bool VisibilityCreateCategory
        {
            get => _visibilityCreateCategory;
            set
            {
                if (value != _visibilityCreateCategory)
                {
                    _visibilityCreateCategory = value;
                    onPropertyChanged(nameof(VisibilityCreateCategory));
                }
            }
        }

        private bool _visibilityUpdateCategory;
        public bool VisibilityUpdateCategory
        {
            get => _visibilityUpdateCategory;
            set
            {
                if (value != _visibilityUpdateCategory)
                {
                    _visibilityUpdateCategory = value;
                    onPropertyChanged(nameof(VisibilityUpdateCategory));
                }
            }
        }

        private bool _newCategoryProfitAnimation;
        public bool NewCategoryProfitAnimation
        {
            get => _newCategoryProfitAnimation;
            set
            {
                
                    _newCategoryProfitAnimation = value;
                    onPropertyChanged(nameof(NewCategoryProfitAnimation));
                
            }
        }

        private bool _newCategoryCostsAnimation;
        public bool NewCategoryCostsAnimation
        {
            get => _newCategoryCostsAnimation;
            set
            {
                
                    _newCategoryCostsAnimation = value;
                    onPropertyChanged(nameof(NewCategoryCostsAnimation));
                
            }
        }
        private bool _visibilityPopup;
        public bool VisibilityPopup
        {
            get => _visibilityPopup;
            set
            {
                if (value != _visibilityPopup)
                {
                    _visibilityPopup = value;
                    onPropertyChanged(nameof(VisibilityPopup));
                }
            }
        }
        #endregion

        #region Command
        public void ClosePopup()
        {
            VisibilityPopup = false;
        }
        public void ShowPopup()
        {
            VisibilityPopup = true;
        }

        public void SetDefaultProperty()
        {
                NewCategoryCostsAnimation = false;
                NewCategoryProfitAnimation = false;
                ExeptionNameCategory = "";
                ExeptionTypeCategory = "";
                NameNewCategory = "";
                PriceNewCategory = 0;
                ColorNewCategory = "";
                ImageNewCategory = null;
                _isCostsNewCategory = null;
                WidthNewCategoryProfit = 30;
                WidthNewCategoryCosts = 30;
                VisibilityUpdateCategory = false;
                VisibilityCreateCategory = true;
                VisiilityPrice = false;
                onPropertyChanged(nameof(ExeptionNameCategory));
                onPropertyChanged(nameof(ExeptionTypeCategory));
                IsShowAnimateClose = false;     
        }

        public async Task UpdateCategory()
        {
            if (!CheckCreatedCategory())
            {
                onPropertyChanged(nameof(ExeptionTypeCategory));
                onPropertyChanged(nameof(ExeptionNameCategory));
                return ;
            }
           
            Category category = new Category()
            {
                Id = this._data.CurentCategory.Id,
                Name = NameNewCategory,
                Color = ColorNewCategory,
                Price = PriceNewCategory,
                isCosts = _isCostsNewCategory.Value,
                Image = ImageNewCategory,
                ImageName = this._data.CurentCategory.ImageName,
                IsActual = true
            };

            this._data.UpdateCategories(category, ActionType.Update);
            IsShowAnimateClose = true;
        }

        public void SetToUpdate()
        {
            NewCategoryCostsAnimation = false;
            NewCategoryProfitAnimation = false;
            VisibilityUpdateCategory = true;
            VisibilityCreateCategory = false;

            ColorNewCategory = this._data.CurentCategory.Color;
            _isCostsNewCategory = this._data.CurentCategory.isCosts;
            PriceNewCategory = this._data.CurentCategory.Price;
            NameNewCategory = this._data.CurentCategory.Name;
            ImageNewCategory = this._data.CurentCategory.Image;

            if (this._data.CurentCategory.isCosts)
            {
                WidthNewCategoryCosts = 120;
                WidthNewCategoryProfit = 30;
            }
            else
            {
                WidthNewCategoryProfit = 120;
                WidthNewCategoryCosts = 30;
            }
              

            if (this._data.CurentCategory.Price != 0)
                VisiilityPrice = true;
            else
                VisiilityPrice = false;

            IsShowAnimateClose = false;
        }

        public async Task DeleteCategory()
        {
            var rez = MessageBox.Show($"{_language.MessageBoxCategory} {this._data.CurentCategory.Name}?", "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rez == MessageBoxResult.Yes)
            {
                this._data.UpdateCategories(this._data.CurentCategory, ActionType.Remove);
            }
        }


        private async Task<bool> CreateCategory()
        {
           

            if (!CheckCreatedCategory())
            {
                onPropertyChanged(nameof(ExeptionTypeCategory));
                onPropertyChanged(nameof(ExeptionNameCategory));

                return false;
            }
         

            Category category = new Category()
            {
                Id = this._data.MainInfo.LastIdCategory++,
                Name = _nameNewCategory,
                isCosts = _isCostsNewCategory.Value,
                Price = _priceNewCategory,
                Color = _colorNewCategory,
                IsActual = true
            };

            if (ColorNewCategory == null ||  String.IsNullOrEmpty(ColorNewCategory))
            {
                category.Color = "#3498DB";
            }

            if (_imageNewCategory == null)
            {
                category.ImageName = "default.png";
                category.Image = new BitmapImage(new Uri(_repository.GetRootDirectory("Files") + "\\default.png", UriKind.Absolute));
            }
            else
            {
                category.ImageName = _imageNewCategory.UriSource.Segments.Last();
                category.Image = _imageNewCategory;
            }

            this._data.UpdateCategories(category,ActionType.Add);

            IsShowAnimateClose = true;

            return true;
        }


        private void TapCostsNewCategory()
        {
            if (_isCostsNewCategory == true)
            {
                _isCostsNewCategory = null;
                NewCategoryCostsAnimation = false;
            }
            else
            {
                _isCostsNewCategory = true;
                NewCategoryCostsAnimation = true;

                if (NewCategoryProfitAnimation)
                {
                    NewCategoryProfitAnimation = false;
                }
            }
                
        }

        private void TapProfitNewCategory()
        {
            if (_isCostsNewCategory == false)
            {
                _isCostsNewCategory = null;
                NewCategoryProfitAnimation= false;
            }
            else
            {
                _isCostsNewCategory = false;

                NewCategoryProfitAnimation = true;
                if (NewCategoryCostsAnimation)
                {
                    NewCategoryCostsAnimation = false;
                }
            }
               
                
        }


        private bool CheckCreatedCategory()
        {

            ExeptionNameCategory = "";
            ExeptionTypeCategory = "";

            if (_nameNewCategory == null)
            {
                ExeptionNameCategory = _language.ExeptionNameSmallCategory;
                onPropertyChanged(nameof(ExeptionNameCategory));
                return false;
            }


            if (_nameNewCategory.Length > 20)
            {
                ExeptionNameCategory = _language.ExeptionNameBigCategory;
                return false;
            }

            if (_nameNewCategory.Length < 3)
            {
                ExeptionNameCategory = _language.ExeptionNameSmallCategory;
                return false;
            }

            if (Regex.IsMatch(_nameNewCategory, @"[^a-zA-Z0-9\u0400-\u04FF., ] "))
            {
                ExeptionNameCategory = _language.ExeptionNameSymbolCategory;
                return false;
            }


            if (_isCostsNewCategory == null)
            {
                ExeptionTypeCategory = _language.ExeptionTypeCategory;
                return false;
            }

            return true;
        }


        #endregion


        private List<string> SetColors()
        {
            return new List<string>
            {  
               "#7dda58",
               "#f7daff",
               "#ffb347",
               "#45b5aa",
               "#865082",
               "#ae5e54",
               "#36f25c",
               "#7a00eb",
               "#ecb906",
               "#C0392B",
               "#9B59B6",
               "#2980B9",
               "#27AE60",
               "#D35400",
               "#5D6D7E",
               "#fde90d",

             };
        }


    }
}
