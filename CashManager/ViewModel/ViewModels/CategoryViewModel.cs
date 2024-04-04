using CashManager.Model;
using CashManager.Repository;
using GalaSoft.MvvmLight.Command;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CashManager.ViewModel
{
    internal class CategoryViewModel : ViewModelBase
    {
        private readonly DapperRepository _repository = null;


        // 
        public ObservableCollection<Category> Categories { get; }
        public List<string> Colors { get; }
        public List<BitmapImage> ImagesName { get; }
        // Comands
        public ICommand CreateNewCategory { get; private set; }

        public CategoryViewModel(string connectionString) {

            _repository = new DapperRepository(connectionString);

            Categories = new ObservableCollection<Category>(_repository.GetAllCategory());
            Colors = new List<string>();
            ImagesName = new List<BitmapImage>();

            CreateNewCategory = new AsyncCommand(CreateCategory);

            Colors.Add("Blue");
            Colors.Add("Pink");
            Colors.Add("Green");

          
                        
           
            ReadAllPicturesForCreate();
            ReadAllCreatedPictures();
           
        }

        private void ReadAllPicturesForCreate()
        {
            DirectoryInfo dir = new DirectoryInfo(GetRootDirectory());
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                ImagesName.Add(new BitmapImage(new Uri(file.FullName, UriKind.Absolute)));
            }
        }
        private void ReadAllCreatedPictures()
        {
            DirectoryInfo dir = new DirectoryInfo(GetRootDirectory());
            
            foreach(Category category in Categories)
            {
                category.Image = new BitmapImage(new Uri($"{dir.FullName}//{category.ImageName}", UriKind.Absolute));
            }
        }

        private string GetRootDirectory()
        {
            string dirrr = Directory.GetCurrentDirectory();
            return dirrr.Remove(dirrr.IndexOf("bin")) + "Images";
        }

        private async Task<bool> CreateCategory()
        {

            if (_nameCategory.Length == 0)
            {
                return false;
            }

            Category category = new Category() { isCosts = IsCostsCategory, Name = _nameCategory, Price = _priceCategory, Color = _colorCategory };

            if (_colorCategory == null)
            {
                category.Color = "#a23a96";
            }

            if(_imageCategory == null)
            {
                category.ImageName = "default.png";
                category.Image = new BitmapImage(new Uri(GetRootDirectory() + "default.png", UriKind.Absolute));
            }
            else
            {
                string temp = _imageCategory.UriSource.ToString();
                category.ImageName = temp.Remove(0, temp.LastIndexOf("/") + 1);
                category.Image = _imageCategory;
            }

           
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

        private string _colorCategory;
        public string ColorCategory { get=> _colorCategory; set => _colorCategory = value; }

        private BitmapImage _imageCategory;
        public BitmapImage ImageCategory { get => _imageCategory; set => _imageCategory = value; }

       
        //public ICommand CreateCategory
        //{
        //    get => new RelayCommand(() => Categories.Add(new Category() { Id = 1, Name = _nameCategory, Price = _priceCategory }));
        //}

    }
}
