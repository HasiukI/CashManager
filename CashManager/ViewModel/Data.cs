using CashManager.Model;
using CashManager.Repository;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CashManager.ViewModel
{
    internal class Data :  ViewModelBase
    {
        private readonly IRepository _repository;
        private readonly LanguagesViewModel _languages;

        public List<Category> AllCategories;
        public ObservableCollection<HistoryDay> DaysHistory { get; set; }
        public ObservableCollection<Category> CategoriesProfit { get; set; }
        public ObservableCollection<Category> CategoriesCosts { get; set; }
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ISeries> SeriesCostsCircle { get; set; }
        public ObservableCollection<ISeries> SeriesProfitCircle { get; set; }
        public MainInfo MainInfo { get; set; }

        public Data(LanguagesViewModel languages,IRepository repository) {
            _repository = repository;
            _languages = languages;

            Loaded();


        }

        #region Property
        public bool IsSelectedTypeCategoryCosts { get; set; }

        private HistoryDay _selectedDayHistory;
        public HistoryDay SelectedDayHistory
        {
            get => _selectedDayHistory;
            set
            {
                if (value != _selectedDayHistory)
                {
                    _selectedDayHistory = value;
                    SelectedDate = _selectedDayHistory.Date;
                    MainDayOfWeek = "";
                    onPropertyChanged(nameof(SelectedDate));
                    onPropertyChanged(nameof(SelectedDayHistory));
                }
            }
        }

        public DateTime SelectedDate { get; set; }

        private Category _curentCategory;
        public Category CurentCategory
        {
            get => _curentCategory;
            set
            {

                if (value != _curentCategory)
                {
                    _curentCategory = value;
                    onPropertyChanged(nameof(CurentCategory));
                }

            }
        }

        private string _mainDayOfWeek;
        public string MainDayOfWeek
        {
            get => _mainDayOfWeek;
            set
            {
                if (value != _mainDayOfWeek)
                {
                    _mainDayOfWeek = _languages.GetDayOfWeek((int)SelectedDate.DayOfWeek);
                    onPropertyChanged(nameof(MainDayOfWeek));
                }
            }
        }

        private string _curentNameCategory;
        public string CurentNameCategory
        {
            get => _curentNameCategory;
            set
            {
                if (IsSelectedTypeCategoryCosts)
                    _curentNameCategory = _languages.CategoryCosts;
                else
                    _curentNameCategory = _languages.CategoryProfit;
                onPropertyChanged(nameof(CurentNameCategory));
            }
        }

        private string _noCurentNameCategory;
        public string NoCurentNameCategory
        {
            get => _noCurentNameCategory;
            set
            {
                if (IsSelectedTypeCategoryCosts)
                    _noCurentNameCategory = _languages.CategoryProfit;
                else
                    _noCurentNameCategory = _languages.CategoryCosts;
                onPropertyChanged(nameof(NoCurentNameCategory));
            }
        }
        #endregion


        #region Updates

        public void UpdateDiagram(DateTime date)
        {
            Series.Clear();
            SeriesCostsCircle.Clear();
            SeriesProfitCircle.Clear();

            List<AllCashInCategory> Items = _repository.GetDiagram(date,AllCategories).ToList();

            foreach (var item in Items)
            {

                Series.Add(new ColumnSeries<double>
                {
                    Values = new ObservableCollection<double> {(item.Category.isCosts)? (double)-item.TotalSum : (double)item.TotalSum },
                    Fill = new SolidColorPaint(SKColor.Parse(item.Category.Color)), 
                    Stroke = null,
                    //DataLabelsSize = 16,
                    //DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    //DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Middle,
                    DataLabelsFormatter = (point) => item.Category.Name,
                    Name=item.Category.Name,
                    
                });

                if (item.Category.isCosts)
                {
                    SeriesCostsCircle.Add(new PieSeries<double>
                    {
                        Values = new double[] { (double)item.TotalSum },
                        Fill = new SolidColorPaint(SKColor.Parse(item.Category.Color)),
                        //DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        //DataLabelsSize = 16,
                        //DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        //DataLabelsFormatter = point => item.Category.Name,
                        Name = item.Category.Name
                    }) ;
                }
                else
                {
                    SeriesProfitCircle.Add(new PieSeries<double> { 
                        Values = new double[] { (double)item.TotalSum },
                        Fill = new SolidColorPaint(SKColor.Parse(item.Category.Color)),
                        //DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        //DataLabelsSize = 16,
                        //DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                       // DataLabelsFormatter = point => item.Category.Name,
                        Name = item.Category.Name
                    });
                }
                
            }

        }

        public async Task UpdateCategories(Category category, ActionType type)
        {
            switch (type)
            {
                case ActionType.Add:
                    _repository.UpdateMainInfo(MainInfo);
                    AllCategories.Add(category);
                    await _repository.CreateCategoryAsync(category);
                    break;

                case ActionType.Remove:
                    AllCategories.Remove(category);
                    _repository.UpdateOrDeleteCategoryAsync(AllCategories);
                    break;
                   
                case ActionType.Update:
                    AllCategories[AllCategories.IndexOf(AllCategories.Where(c => c.Id == category.Id).First())] = category;
                    await _repository.UpdateOrDeleteCategoryAsync(AllCategories);
                    break;
            }

            UpdateCategories(category);
            UpdateDiagram(SelectedDate);
        }

        private void UpdateCategories(Category category)
        {
            if (category.isCosts)
            {
                CategoriesCosts.Clear();
                foreach(var cat in AllCategories.Where(c => c.isCosts==true && c.IsActual))
                {
                    CategoriesCosts.Add(cat);
                }
            }
            else
            {
                CategoriesProfit.Clear();
                foreach (var cat in AllCategories.Where(c => c.isCosts==false && c.IsActual))
                {
                    CategoriesProfit.Add(cat);
                }
            }
        }

        public async Task UpdateCash(Cash cash, ActionType action)
        {
            HistoryDay hs = DaysHistory.Where(d => d != null && d.Date.Date == cash.CreatedAt.Date).FirstOrDefault();

            switch (action) {
                case ActionType.Add:
                    if (hs.HistoriesInDay == null)
                        hs.HistoriesInDay = new System.Collections.ObjectModel.ObservableCollection<AllInfoForCash>();

                    hs.HistoriesInDay.Add(new AllInfoForCash() { Cash = cash, Category = CurentCategory });

                    if (CurentCategory.isCosts)
                    {
                        hs.TotalCosts += cash.Price;
                        MainInfo.TotalSum -= cash.Price;
                    }
                    else
                    {
                        hs.TotalProfit += cash.Price;
                        MainInfo.TotalSum += cash.Price;
                    }
                    await _repository.UpdateMainInfo(MainInfo);
                    await _repository.CreateCashAsync(cash);
                    break;

                case ActionType.Remove:

                    var oldCash = hs.HistoriesInDay.Where(h => h.Cash.Id == cash.Id).FirstOrDefault();

                    if (oldCash == null)
                    {
                        return;
                    }

                    if (oldCash.Category.isCosts)
                    {
                        hs.TotalCosts -= oldCash.Cash.Price;
                        MainInfo.TotalSum += oldCash.Cash.Price;
                    }
                    else
                    {
                        hs.TotalProfit -= oldCash.Cash.Price;
                        MainInfo.TotalSum -= oldCash.Cash.Price;
                    }

                    hs.HistoriesInDay.Remove(oldCash);
                    _repository.DeleteCashAsync(cash);
                    break;

                case ActionType.Update:
                    var toUpdateCash = hs.HistoriesInDay.Where(h => h.Cash.Id == cash.Id).FirstOrDefault();

                    if (toUpdateCash == null)
                    {
                        return;
                    }

                    if (toUpdateCash.Category.isCosts)
                    {
                        hs.TotalCosts -= toUpdateCash.Cash.Price;
                        hs.TotalCosts += cash.Price;

                        MainInfo.TotalSum += toUpdateCash.Cash.Price;
                        MainInfo.TotalSum -= cash.Price;
                    }
                    else
                    {
                        hs.TotalProfit -= toUpdateCash.Cash.Price;
                        hs.TotalProfit += cash.Price;

                        MainInfo.TotalSum -= toUpdateCash.Cash.Price;
                        MainInfo.TotalSum += cash.Price;
                    }
                    int pos = hs.HistoriesInDay.IndexOf(toUpdateCash);
                    hs.HistoriesInDay.Remove(toUpdateCash);
                    hs.HistoriesInDay.Insert(pos, new AllInfoForCash() { Cash=cash, Category= toUpdateCash.Category});
                    _repository.UpdateCashAsync(cash);

                    break;       
            }

            _repository.UpdateMainInfo(MainInfo);

            UpdateDiagram(SelectedDate);
        }

        public void ShowInfoByMount(DateTime date)
        {
            DaysHistory.Clear();
            foreach (HistoryDay day in _repository.GetAllHistoryInMounth(date,AllCategories))
            {
                DaysHistory.Add(day);
            }

            UpdateDiagram(date);
        }

       
        public void UpdateView()
        {
            MainDayOfWeek = "";
            CurentNameCategory = "";
            MainInfo.Language = _languages.CurentNameLanguage;
            _repository.UpdateMainInfo(MainInfo);
        }
        #endregion

        #region SetDefult
        private void Loaded()
        {
            MainInfo = _repository.GetMainInfo();
            SelectedDate = DateTime.Now;
            MainDayOfWeek = "";

            AllCategories = _repository.GetAllCategory().ToList();


            Series = new ObservableCollection<ISeries>();
            SeriesCostsCircle = new ObservableCollection<ISeries>();
            SeriesProfitCircle = new ObservableCollection<ISeries>();
            UpdateDiagram(SelectedDate);


            IsSelectedTypeCategoryCosts = false;
            CurentNameCategory = "";
            NoCurentNameCategory = "";
            CategoriesProfit = new ObservableCollection<Category>(AllCategories.Where(c => c.IsActual && !c.isCosts));
            CategoriesCosts = new ObservableCollection<Category>(AllCategories.Where(c => c.IsActual && c.isCosts));
           
        }
        #endregion
    }

    enum ActionType
    {
        Add,
        Remove,
        Update
    }
   
}
