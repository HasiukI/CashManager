using CashManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Model
{
    internal class MainInfo : ViewModelBase
    {
        private int _lastIdCategory;
        public int LastIdCategory { 
            get=>_lastIdCategory;
            set {
                if(_lastIdCategory != value)
                {
                    _lastIdCategory = value;
                    onPropertyChanged(nameof(LastIdCategory));
                }
            }
        }
        private int _lastIdCash;
        public int LastIdCash
        {
            get => _lastIdCash;
            set
            {
                if (_lastIdCash != value)
                {
                    _lastIdCash = value;
                    onPropertyChanged(nameof(LastIdCash));
                }
            }
        }

        private decimal _totalSum;
        public decimal TotalSum { 
            get=>_totalSum;
            set { 
                if(value != _totalSum)
                {
                    _totalSum = value;
                    onPropertyChanged(nameof(TotalSum));
                }
            }
        }


        public string PickedColor { get; set; }
        public string PickedImageName { get; set; }
        public int LastIdBackGround { get; set; }
        public string Language { get; set; }
    }
}
