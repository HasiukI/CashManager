using CashManager.Model;
using CashManager.Repository;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CashManager.ViewModel
{
    internal class CashViewModel : ViewModelBase
    {
        private readonly Data _data = null;
        private readonly IRepository _repository;
        private readonly LanguagesViewModel _language;

        public ICommand CreateCashCommand { get; }
        public ICommand DeleteCashCommand { get; }
        public ICommand UpdateCashCommand { get; }
        public ICommand SetToUpdateCashCommand { get; }
        public ICommand SetDefaultPropertyCommand { get; }
        public ICommand UpCountCommand { get; }
        public ICommand DownCountCommand { get; }
        public ICommand ShowPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }

        public CashViewModel(Data data, IRepository repository, LanguagesViewModel language)
        {
            _data = data;
            _repository = repository;
            _language = language;

            CreateCashCommand = new AsyncCommand(CreateCash);
            DeleteCashCommand = new AsyncCommand(DeleteCash);
            SetToUpdateCashCommand = new Command(SetToUpdate);
            UpdateCashCommand = new AsyncCommand(UpdateCash);
            SetDefaultPropertyCommand = new Command(SetDefaultProperty);
            UpCountCommand = new Command(UpCount);
            DownCountCommand = new Command(DownCount);
            ShowPopupCommand = new Command(ShowPopup);
            ClosePopupCommand = new Command(ClosePopup);
        }

        #region Property

        private string _exeptionCash;
        public string ExeptionCash { 
            get=>_exeptionCash;
            set { 
                if(value != _exeptionCash)
                {
                    _exeptionCash= value;
                    onPropertyChanged(nameof(ExeptionCash));
                }
            } 
        }

        private AllInfoForCash _selectedHistory;
        public AllInfoForCash SelectedHistory
        {
            get => _selectedHistory;
            set
            {
                if (_selectedHistory != value)
                {
                    _selectedHistory = value;

                    if (_selectedHistory != null)
                    {
                        this._data.CurentCategory = _selectedHistory.Category;
                        SetToUpdate();
                    }
                         
                    onPropertyChanged(nameof(SelectedHistory));
                }
            }
        }

        private int _count;
        public int Count { 
            get=>_count;
            set { 
                if(value != _count)
                {
                    _count = value;
                    onPropertyChanged(nameof(Count));
                }
            }
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get => _totalPrice;
            set
            {
               if (_totalPrice != value)
                {
                    if (!string.IsNullOrEmpty(value) &&  Regex.IsMatch(value, @"[^0-9]"))
                    {
                        ExeptionCash = _language.CreateCashSum;
                    }
                    else
                    {
                        _totalPrice = value;
                        ExeptionCash = "";
                        onPropertyChanged(nameof(TotalPrice));
                    }
                    
                }
            }
        }

        private string _cashDescription;
        public string CashDescription {
            get => _cashDescription;
            set
            {
                if (value!=_cashDescription)
                {
                    _cashDescription = value;
                    onPropertyChanged($"{nameof(CashDescription)}");

                }
            }
        }

        private bool _visibilityDescription;
        public bool VisibilityDescription { 
            get=> _visibilityDescription;
            set { 
                if( value!= _visibilityDescription)
                {
                    _visibilityDescription = value;
                    onPropertyChanged(nameof(VisibilityDescription));
                }
            } 
        }

        private bool _visibilityCreateCash;
        public bool VisibilityCreateCash
        {
            get => _visibilityCreateCash;
            set
            {
                if (value != _visibilityCreateCash)
                {
                    _visibilityCreateCash = value;
                    onPropertyChanged(nameof(VisibilityCreateCash));
                }
            }
        }

        private bool _visibilityUpdateCash;
        public bool VisibilityUpdateCash
        {
            get => _visibilityUpdateCash;
            set
            {
                if (value != _visibilityUpdateCash)
                {
                    _visibilityUpdateCash = value;
                    onPropertyChanged(nameof(VisibilityUpdateCash));
                }
            }
        }

        private bool _isShowAnimate;
        public bool IsShowAnimate
        {
            get => _isShowAnimate;
            set
            {
                if(value != _isShowAnimate)
                {
                    _isShowAnimate = value;
                    onPropertyChanged(nameof(IsShowAnimate));
                }
            }
        }

        private bool _visibilityPopup;
        public bool VisibilityPopup
        {
            get => _visibilityPopup;
            set
            {
                if (_visibilityPopup != value)
                {
                    _visibilityPopup = value;
                    onPropertyChanged(nameof(VisibilityPopup));
                }
            }
        }
        #endregion


        #region Command

        public void ShowPopup()
        {
            VisibilityPopup = true;
        }
        public void ClosePopup()
        {
            VisibilityPopup = false;
        }

        public void SetDefaultProperty()
        {
            ExeptionCash = "";
            CashDescription = String.Empty;
            Count = 0;
            TotalPrice = string.Empty;
            if (this._data.CurentCategory.Price != 0)
            {
                Count = 1;
                TotalPrice = this._data.CurentCategory.Price.ToString();
            }

            VisibilityCreateCash = true;
            VisibilityUpdateCash = false;
            IsShowAnimate = false;
        }

        public void UpCount()
        {
                Count++;
                TotalPrice = (Count * this._data.CurentCategory.Price).ToString();
        }

        public void DownCount()
        {
                if (Count > 1)
                {
                    Count--;
                    TotalPrice = (Count * this._data.CurentCategory.Price).ToString();
                }
        }


        private void SetToUpdate()
        {
            ExeptionCash = "";
            TotalPrice = SelectedHistory.Cash.Price.ToString();
            CashDescription = SelectedHistory.Cash.Description;

            if (SelectedHistory.Category.Price != 0)
            {
                this.Count = _selectedHistory.Cash.Count;
            }
            else
            {
                Count = 0;
            }

            VisibilityCreateCash = false;
            VisibilityUpdateCash = true;
            IsShowAnimate = false;
        }

        private async Task<bool> CreateCash()
        {


            if (!CheckCreatedCash())
            {
                return false;
            }
           

            Cash cash = new Cash() { 
                Id=this._data.MainInfo.LastIdCash++, 
                CategoryId = this._data.CurentCategory.Id, 
                Price = decimal.Parse(TotalPrice), 
                CreatedAt = this._data.SelectedDate.Date.Add(DateTime.Now.TimeOfDay),
                Description= this.CashDescription
            };

            if (this._data.CurentCategory.Price != 0)
                cash.Count = Count;

            this._data.UpdateCash(cash, ActionType.Add);

            IsShowAnimate = true;
            return true;
        }
        
        private async Task DeleteCash()
        {
            var rez = MessageBox.Show($"{_language.MessageBoxCash} {SelectedHistory.Cash.Price}?", "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rez == MessageBoxResult.Yes)
            {
                this._data.UpdateCash(SelectedHistory.Cash, ActionType.Remove);
            }

        }

        private async Task UpdateCash()
        {
            if (SelectedHistory == null)
                return;

            if (!CheckCreatedCash())
            {
                return ;
            }

            Cash cash = new Cash()
            {
                Id = this.SelectedHistory.Cash.Id,
                CategoryId = this.SelectedHistory.Cash.CategoryId,
                Price = decimal.Parse(this.TotalPrice),
                CreatedAt = this.SelectedHistory.Cash.CreatedAt,
                Count = this.Count,
                Description = this.CashDescription

            };

            this._data.UpdateCash(cash, ActionType.Update);
            IsShowAnimate = true;
        }

     
        private bool CheckCreatedCash()
        {
            ExeptionCash = "";
            onPropertyChanged(nameof(ExeptionCash));

            if (decimal.Parse(TotalPrice) <= 0)
            {
                ExeptionCash = _language.ExeptionCreateCashValue;
                onPropertyChanged(nameof(ExeptionCash));
                return false;
            }
            if (CashDescription == null)
            {
                CashDescription = "none";
            }
            if(Regex.IsMatch(CashDescription, @"[^a-zA-Z0-9\u0400-\u04FF., ]"))
            {
                ExeptionCash = _language.ExeptionCreateCashDescription;
                onPropertyChanged(nameof(ExeptionCash));
                return false;
            }
            if(this._data.SelectedDate.Date > DateTime.Now.Date)
            {
                ExeptionCash = _language.ExeptionCreateCashDate;
                onPropertyChanged(nameof(ExeptionCash));
                return false;
            }

                return true;
        }
        #endregion



    }
}
