using CashManager.Model;
using CashManager.Repository;
using CashManager.ViewModel;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CashManager.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private readonly DapperRepository _repository = null;


        // 
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<History> Histories { get; private set; }
        public ObservableCollection<TotalSumDay> TotalSumDays { get; private set; }
        public List<string> Colors { get; }
        public List<BitmapImage> Images { get; }
        public DateTime SelectedMounthDate { get; private set; }
       

        // Comands
        public ICommand CreateNewCategory { get; private set; }
        public ICommand CreateCashItem { get; private set; }
        public ICommand CountUpCash { get; private set; }
        public ICommand CountDownCash { get; private set; }
        public ICommand NextMounth { get; private set; }
        public ICommand PastMounth { get; private set; }
        public ICommand TapCosts { get; private set; }
        public ICommand TapProfit { get; private set; }

        public MainViewModel()
        {
            SelectedMounthDate = DateTime.Now;
           

            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            _repository = new DapperRepository(connectionString);

            Categories = new ObservableCollection<Category>(_repository.LoadCategories());
            Histories = new ObservableCollection<History>(_repository.LoadHistory(SelectedMounthDate));
            TotalSumDays = new ObservableCollection<TotalSumDay>(LoadCalendar(SelectedMounthDate));
            Colors = new List<string>();
            Images = _repository.ReadAllStaticPictures();

            SelectedDate = TotalSumDays.Where(t => t != null && t.CurentDate == DateTime.Now).FirstOrDefault();

            CreateNewCategory = new AsyncCommand(CreateCategory);
            CreateCashItem = new AsyncCommand(CreateCash);
            CountUpCash = new Command(CountUp);
            CountDownCash = new Command(CountDown);
            NextMounth = new Command(NextMounthCheck);
            PastMounth = new Command(PastMounthCheck);
            TapCosts = new Command(TapCostsCreateCategory);
            TapProfit = new Command(TapProfitCreateCategory);

            CountCash = 1;
           

            Colors.Add("#39878c");
            Colors.Add("#fe4505");
            Colors.Add("#ffc117");

            _isCosts = true;
        }

        private void TapCostsCreateCategory()
        {
            _isCosts = true;
        }
        private void TapProfitCreateCategory()
        {
            _isCosts = false;
        }

        private void  NextMounthCheck()
        {
            SelectedMounthDate = SelectedMounthDate.AddMonths(1);
            TotalSumDays.Clear();
            foreach(TotalSumDay total in LoadCalendar(SelectedMounthDate))
            {
                TotalSumDays.Add(total);
            }
            onPropertyChanged(nameof(SelectedMounthDate));
        }
        private void PastMounthCheck()
        {
            SelectedMounthDate = SelectedMounthDate.AddMonths(-1);
            TotalSumDays.Clear();
            foreach (TotalSumDay total in LoadCalendar(SelectedMounthDate))
            {
                TotalSumDays.Add(total);
            }
            onPropertyChanged(nameof(SelectedMounthDate));
        }

        /// <summary>
        /// Check history and get total sum
        /// </summary>
        /// <param name="date"></param>
        private List<TotalSumDay> LoadCalendar(DateTime date)
        {
            List<TotalSumDay> days = new List<TotalSumDay>();

            List<History> histories = Histories.Where(h=>h.Cash.CreatedAt.Month == date.Month && h.Cash.CreatedAt.Year == date.Year).ToList();

          

            for(int i = 0; i < DateTime.DaysInMonth(date.Year,date.Month); i++) {
                

                List<History> dayhistory = histories.Where(h =>h.Cash.CreatedAt.Day == i+1).ToList(); 
                
                if(dayhistory.Count > 0)
                {
                    decimal totalProfit = 0;
                    decimal totalCosts = 0;

                    foreach(History h in dayhistory)
                    {

                        if (h.Category.isCosts)
                        {
                            totalProfit += h.Cash.Price;
                        }
                        else
                        {
                            totalCosts += h.Cash.Price;
                        }
                    }

                    days.Add(new TotalSumDay() { TotalCosts = totalCosts, TotalProfit = totalProfit, CurentDate = dayhistory[0].Cash.CreatedAt });
                }
                else
                {
                    days.Add(new TotalSumDay() { CurentDate= new DateTime(date.Year,date.Month,i+1)});
                }
            }
            return days;
        }

        private async Task<bool> CreateCategory()
        {

            if (_nameCategory.Length == 0)
            {
                return false;
            }

            Category category = new Category() { isCosts = _isCosts, Name = _nameCategory, Price = _priceCategory, Color = _colorCategory, IsActual = true };

            if (_colorCategory == null)
            {
                category.Color = "#a23a96";
            }

            if (_imageCategory == null)
            {
                category.ImageName = "default.png";
                category.Image = new BitmapImage(new Uri(_repository.GetRootDirectoryImages() + "\\default.png", UriKind.Absolute));
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


        //ForCreateCategory
        private string _nameCategory;
        public string NameCategory { get => _nameCategory; set => _nameCategory = value; }

        private int _priceCategory;
        public int PriceCategory { get => _priceCategory; set => _priceCategory = value; }

        private bool _isCosts;

        private string _colorCategory;
        public string ColorCategory { get => _colorCategory; set => _colorCategory = value; }

        private BitmapImage _imageCategory;
        public BitmapImage ImageCategory { get => _imageCategory; set => _imageCategory = value; }

        //ForCreateCash
        public decimal TotalCash { get; private set; }
        public int CountCash { get; private set; }

        private Category _curentCategory;
        public Category CurentCategory
        {
            get => _curentCategory;
            set
            {
                _curentCategory = value;
                TotalCash = _curentCategory.Price;
                onPropertyChanged(nameof(TotalCash));
                onPropertyChanged(nameof(CurentCategory));
            }
        }
        private TotalSumDay _selectedDate;
        public TotalSumDay SelectedDate {
            get=> _selectedDate;
            set { 
                if(_selectedDate != value && value.CurentDate<=DateTime.Now)
                {
                    _selectedDate = value;
                    onPropertyChanged(nameof(SelectedDate));
                }
            } 
        }


        public async Task DeleteCategory()
        {
            await _repository.DeleteCategoryAsync(_curentCategory);
            Categories.Remove(_curentCategory);
        }



        private void CountUp()
        {
            TotalCash = (int)_curentCategory.Price * ++CountCash;
            onPropertyChanged(nameof(TotalCash));
            onPropertyChanged(nameof(CountCash));
        }

        private void CountDown()
        {
            if (CountCash > 1)
            {
                TotalCash = (int)_curentCategory.Price * (--CountCash);
                onPropertyChanged(nameof(TotalCash));
                onPropertyChanged(nameof(CountCash));
            }
        }



        /// <summary>
        /// Create Cash and Add to history adn total
        /// </summary>
        /// <returns></returns>
        private async Task CreateCash()
        {
            Cash cash = new Cash() { Price = TotalCash, CategoryId = _curentCategory.Id, CreatedAt = _selectedDate.CurentDate };
            await _repository.CreateCashAsync(cash);

            Histories.Add(new History() { Cash = cash, Category=_curentCategory });

            var totalSumDay = TotalSumDays.Where(h => h.CurentDate.Date == _selectedDate.CurentDate.Date).FirstOrDefault();

            if (_curentCategory.isCosts)
            {
                totalSumDay.TotalCosts += cash.Price;
            }
            else
            {
                totalSumDay.TotalProfit += cash.Price;
            }   
        }
    }
}
