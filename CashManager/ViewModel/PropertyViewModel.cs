using CashManager.Model;
using CashManager.Repository;
using Microsoft.Win32;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CashManager.ViewModel
{
    class PropertyViewModel : ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly Data _data;
        public List<ColorsDesign> BackgroundColor { get; private set; }
        public ObservableCollection<BitmapImage> BackGroundsPictures { get; private set; }


        public ICommand SetNullImageCommand { get; }
        public ICommand AddImageBackGroundCommand {  get; }
        public ICommand DeleteImageBackGroundCommand { get; }
        public ICommand HideOrShowProfitCategorriesCommand { get; }
        public ICommand HideOrShowCostsCategorriesCommand { get; }
        public ICommand ShowCategoryCommand { get; }
        public ICommand ShowCashCommand { get; }
        public ICommand ShowPropertyCommand { get; }
        public ICommand ShowPropertyOther { get; }
        public ICommand SendMessageMeCommand { get; }
        public ICommand SendMessageDanaCommand { get; }
       

        public PropertyViewModel(Data data, IRepository repository)
        {
            _repository = repository;
            _data = data;

            SetDefaultColor();
            SetDefultBackGroundAndTheme();

            SetNullImageCommand = new Command(SetNullImage);
            AddImageBackGroundCommand = new AsyncCommand(AddImageBackGround);
            DeleteImageBackGroundCommand = new AsyncCommand(DeleteImageBackGround);
            HideOrShowProfitCategorriesCommand = new Command(HideOrShowProfitCategorries);
            HideOrShowCostsCategorriesCommand = new Command(HideOrShowCostsCategorries);
            ShowCategoryCommand = new Command(ShowCategory);
            ShowCashCommand = new Command(ShowCash);
            ShowPropertyCommand = new Command(ShowProperty);
            ShowPropertyOther= new Command(ShowOther);
            SendMessageMeCommand = new Command(SendMessageMe);
            SendMessageDanaCommand = new Command(SendMessageDana);
        }

        #region Property
      


        private bool _isHideCategoriesProfit;
        public bool IsHideCategoriesProfit
        {
            get => _isHideCategoriesProfit;
            set
            {
                if (_isHideCategoriesProfit != value)
                {
                    _isHideCategoriesProfit = value;
                    onPropertyChanged(nameof(IsHideCategoriesProfit));
                }
            }
        }

        private bool _isHideCategoriesCosts;
        public bool IsHideCategoriesCosts
        {
            get => _isHideCategoriesCosts;
            set
            {
                if (_isHideCategoriesCosts != value)
                {
                    _isHideCategoriesCosts = value;
                    onPropertyChanged(nameof(IsHideCategoriesCosts));
                }
            }
        }

        private ColorsDesign _curentColor;
        public ColorsDesign CurentColor
        {
            get => _curentColor;
            set
            {
                if (value != _curentColor && value != null)
                {

                    _curentColor = value;
                    _data.MainInfo.PickedColor = _curentColor.MainColor;
                    this._repository.UpdateMainInfo(this._data.MainInfo);
                    onPropertyChanged(nameof(CurentColor));
                }

            }
        }

        private BitmapImage _curentBackGroundPicture;
        public BitmapImage CurentBackGroundPicture
        {
            get => _curentBackGroundPicture;
            set
            {
                if (value != _curentBackGroundPicture)
                {
                    _curentBackGroundPicture = value;
                    if (value == null)
                    {
                        _data.MainInfo.PickedImageName = "none";
                    }
                    else
                    {
                        _data.MainInfo.PickedImageName = _curentBackGroundPicture.UriSource.Segments.Last();
                    }
                    this._repository.UpdateMainInfo(this._data.MainInfo);
                    onPropertyChanged(nameof(CurentBackGroundPicture));
                }

            }
        }
        private int _zIndexCategory;
        public int ZIndexCategory
        {
            get => _zIndexCategory;
            set
            {
                if (_zIndexCategory != value)
                {
                    _zIndexCategory = value;
                    onPropertyChanged(nameof(ZIndexCategory));
                }
            }
        }

        private int _zIndexOther;
        public int ZIndexOther
        {
            get => _zIndexOther;
            set
            {
                if (_zIndexOther != value)
                {
                    _zIndexOther = value;
                    onPropertyChanged(nameof(ZIndexOther));
                }
            }
        }
        private int _zIndexCash;
        public int ZIndexCash
        {
            get => _zIndexCash;
            set
            {
                if (_zIndexCash != value)
                {
                    _zIndexCash = value;
                    onPropertyChanged(nameof(ZIndexCash));
                }
            }
        }
        private int _zIndexProperty;
        public int ZIndexProperty
        {
            get => _zIndexProperty;
            set
            {
                if (_zIndexProperty != value)
                {
                    _zIndexProperty = value;
                    onPropertyChanged(nameof(ZIndexProperty));
                }
            }
        }
        #endregion

        #region Command

        private void SendMessageMe()
        {
            Process.Start(new ProcessStartInfo("mailto:hasiukiv@gmail.com") { UseShellExecute = true });
        }
        private void SendMessageDana()
        {
            Process.Start(new ProcessStartInfo("mailto:antoniukdana0608@gmail.com") { UseShellExecute = true });
        }

        private void ShowOther()
        {
            ZIndexOther = 3;
            ZIndexCash = 2;
            ZIndexCategory = 2;
            ZIndexProperty = 2;
        }
        private void ShowProperty() {
            ZIndexCash = 2;
            ZIndexCategory = 2;
            ZIndexProperty = 3;
            ZIndexOther = 2;
        }
        private void ShowCash() {
            ZIndexCash = 3;
            ZIndexCategory = 2;
            ZIndexProperty = 2;
            ZIndexOther = 2;
        }
        private void ShowCategory() {
            ZIndexCash = 2;
            ZIndexCategory = 3;
            ZIndexProperty = 2;
            ZIndexOther = 2;
        }

        private void HideOrShowProfitCategorries()
        {
            IsHideCategoriesProfit = !IsHideCategoriesProfit;
        }

        private void HideOrShowCostsCategorries()
        {
            IsHideCategoriesCosts = !IsHideCategoriesCosts;
        }

        private async Task DeleteImageBackGround()
        {
            string path = CurentBackGroundPicture.UriSource.LocalPath;


            BackGroundsPictures.Remove(CurentBackGroundPicture);
            CurentBackGroundPicture = null;
            await _repository.DeleteBackGround(path);
        }

        private void SetNullImage()
        {
            CurentBackGroundPicture = null;
        }

        private async Task AddImageBackGround()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {

                _data.MainInfo.LastIdBackGround++;
                BackGroundsPictures.Add(await _repository.AddBackGround(openFileDialog.FileName, _data.MainInfo.LastIdBackGround));
                await _repository.UpdateMainInfo(_data.MainInfo);
            }

        }

        #endregion





        #region SeterDefault
        private void SetDefaultColor()
        {
            BackgroundColor = new List<ColorsDesign>();

            BackgroundColor.Add(new ColorsDesign() { LightColor = "#eff4f9", MainColor = "#d0dfed", HardColor = "#6e93c3", HardColorEnter = "#8aaed0", ColorForFontsOnBlack = "#f3f8fb", ColorForFontsOnWhite = "#3b4c6d" });
            BackgroundColor.Add(new ColorsDesign() { LightColor = "#fde6f1", MainColor = "#fecce3", HardColor = "#f73d85", HardColorEnter = "#fd69a6", ColorForFontsOnBlack = "#fef1f7", ColorForFontsOnWhite = "#e71b60" });
            BackgroundColor.Add(new ColorsDesign() { LightColor = "#454545", MainColor = "#333333", HardColor = "#4f4f4f", HardColorEnter = "#5d5d5d", ColorForFontsOnBlack = "#f6f6f6", ColorForFontsOnWhite = "#e7e7e7" });

        }

        private void SetDefultBackGroundAndTheme()
        {

            ColorsDesign curentColor = BackgroundColor.Where(c => c.MainColor == _data.MainInfo.PickedColor).FirstOrDefault();
            if (curentColor != null)
                CurentColor = curentColor;
            else
                CurentColor = BackgroundColor[0];


            BackGroundsPictures = new ObservableCollection<BitmapImage>(_repository.GetAllBackGrounds().ToList());
            BitmapImage curentImage = BackGroundsPictures.Where(i => _data.MainInfo.PickedImageName.Equals(i.UriSource.Segments.Last())).FirstOrDefault();
            if (curentImage != null)
                CurentBackGroundPicture = curentImage;
            else
            {
                if (_data.MainInfo.PickedImageName == "none")
                {
                    CurentBackGroundPicture = null;
                }
                else
                {
                    CurentBackGroundPicture = BackGroundsPictures[0];
                }
            }
            ZIndexCash = 2;
            ZIndexCategory = 2;
            ZIndexProperty = 2;
        }
        #endregion

    }
}
