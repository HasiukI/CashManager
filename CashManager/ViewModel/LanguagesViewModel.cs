using CashManager.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace CashManager.ViewModel
{
    internal class LanguagesViewModel : ViewModelBase
    {
        private readonly string[] MounthUkr = { "Січня","Лютого","Березня","Квітня","Травня","Червня","Липня","Серпня","Вересня","Жовтня","Листопада","Грудня" };
        private readonly string[] MounthUkrCalendar = { "Січень","Лютий","Березень","Квітень","Травень","Червень","Липень","Серпень","Вересень","Жовтень","Листопад","Грудень" };
        private readonly string[] MounthEng = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private readonly string[] DayOfWeekEng = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private readonly string[] DayOfWeekUkr = { "Неділя", "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота" };

        private string _curentNameLanguage;
        public string CurentNameLanguage { 
            get=> _curentNameLanguage;
            set
            {
                if(value!= _curentNameLanguage && value != null)
                {
                    _curentNameLanguage = value;
                    CurentLanguage = AllLanguages.Where(l => l.Language == _curentNameLanguage).First();

                    onPropertyChanged(nameof(CategoryProfit));
                    onPropertyChanged(nameof(CategoryCosts));
                    onPropertyChanged(nameof(CreateCategory));
                    onPropertyChanged(nameof(СhoseTypeCategory));
                    onPropertyChanged(nameof(СhoseNameCategory));
                    onPropertyChanged(nameof(PriceCategory));
                    onPropertyChanged(nameof(PriceCategoryInfo));
                    onPropertyChanged(nameof(ColorsCategory));
                    onPropertyChanged(nameof(ImageCategories));
                    onPropertyChanged(nameof(YourCategory));
                    onPropertyChanged(nameof(ChangeCategory));

                    onPropertyChanged(nameof(CreateCash));
                    onPropertyChanged(nameof(CreateCashWriteSum));
                    onPropertyChanged(nameof(CreateCashAdd));
                    onPropertyChanged(nameof(CreateCashChange));
                    onPropertyChanged(nameof(ExeptionCreateCashValue));
                    onPropertyChanged(nameof(ExeptionCreateCashDate));
                    onPropertyChanged(nameof(CreateCashPrice));
                    onPropertyChanged(nameof(CreateCashCount));
                    onPropertyChanged(nameof(CreateCashDescription));
                    onPropertyChanged(nameof(CreateCashDescriptionInfo));

                    onPropertyChanged(nameof(CalendarMon));
                    onPropertyChanged(nameof(CalendarTue));
                    onPropertyChanged(nameof(CalendarWed));
                    onPropertyChanged(nameof(CalendarThu));
                    onPropertyChanged(nameof(CalendarFri));
                    onPropertyChanged(nameof(CalendarSat));
                    onPropertyChanged(nameof(CalendarSun));

                    onPropertyChanged(nameof(HistoryTotalProfit));
                    onPropertyChanged(nameof(HistoryTotalCosts));

                    onPropertyChanged(nameof(PropertyThame));
                    onPropertyChanged(nameof(PropertyImage));
                    onPropertyChanged(nameof(PropertyImageAdd));
                    onPropertyChanged(nameof(PropertyImageNo));
                    onPropertyChanged(nameof(PropertyLanguage));
                    onPropertyChanged(nameof(PropertyDiagram));
                    onPropertyChanged(nameof(PropertyOpus));
                    onPropertyChanged(nameof(PropertyOpus1));
                    onPropertyChanged(nameof(PropertyOpus2));
                    onPropertyChanged(nameof(PropertyOpusHyper1));
                    onPropertyChanged(nameof(PropertyOpusHyper2));

                    onPropertyChanged(nameof(ToolTipCreateCategory));
                    onPropertyChanged(nameof(ToolTipCalendar));
                    onPropertyChanged(nameof(MainTotal));
                    onPropertyChanged(nameof(ToolTipCreateHistory));
                    onPropertyChanged(nameof(Delete));
                    onPropertyChanged(nameof(Update));
                   
                }
            }
        }
        private Languages CurentLanguage { get; set; }
        private List<Languages> AllLanguages;

        public LanguagesViewModel(string defaultLanguage) {       
            AllLanguages = new List<Languages>();

            AllLanguages.Add(GetEngLanguage());
            AllLanguages.Add(GetUkrLanguage());

            CurentNameLanguage = defaultLanguage;
        }

        #region Words
        public string PropertyThame { get => CurentLanguage.Strings["PropertyThame"]; }
        public string PropertyImage { get => CurentLanguage.Strings["PropertyImage"]; }
        public string PropertyImageAdd { get => CurentLanguage.Strings["PropertyImageAdd"]; }
        public string PropertyImageNo { get => CurentLanguage.Strings["PropertyImageNo"]; }
        public string PropertyLanguage { get => CurentLanguage.Strings["PropertyLanguage"]; }
        public string PropertyDiagram { get => CurentLanguage.Strings["PropertyDiagram"]; }
        public string PropertyOpus { get => CurentLanguage.Strings["PropertyOpus"]; }
        public string PropertyOpus1 { get => CurentLanguage.Strings["PropertyOpus1"]; }
        public string PropertyOpus2 { get => CurentLanguage.Strings["PropertyOpus2"]; }
        public string PropertyOpusHyper1 { get => CurentLanguage.Strings["PropertyOpusHyper1"]; }
        public string PropertyOpusHyper2 { get => CurentLanguage.Strings["PropertyOpusHyper2"]; }

        public string CategoryProfit { get => CurentLanguage.Strings["CategoryProfit"]; }
        public string CategoryCosts { get => CurentLanguage.Strings["CategoryCosts"]; }
        public string CreateCategory { get => CurentLanguage.Strings["CreateCategory"]; }
        public string СhoseTypeCategory { get => CurentLanguage.Strings["СhoseTypeCategory"]; }
        public string СhoseNameCategory { get => CurentLanguage.Strings["СhoseNameCategory"]; }
        public string PriceCategory { get => CurentLanguage.Strings["PriceCategory"]; }
        public string PriceCategoryInfo { get => CurentLanguage.Strings["PriceCategoryInfo"]; }
        public string ColorsCategory { get => CurentLanguage.Strings["ColorsCategory"]; }
        public string ImageCategories { get => CurentLanguage.Strings["ImageCategories"]; }
        public string YourCategory { get => CurentLanguage.Strings["YourCategory"]; }
        public string ChangeCategory { get => CurentLanguage.Strings["ChangeCategory"]; }
        public string ExeptionNameSmallCategory { get => CurentLanguage.Strings["ExeptionNameSmallCategory"]; }
        public string ExeptionNameBigCategory { get => CurentLanguage.Strings["ExeptionNameBigCategory"]; }
        public string ExeptionNameSymbolCategory { get => CurentLanguage.Strings["ExeptionNameSymbolCategory"]; }
        public string ExeptionTypeCategory { get => CurentLanguage.Strings["ExeptionTypeCategory"]; }

        public string CreateCash { get => CurentLanguage.Strings["CreateCash"]; }
        public string CreateCashWriteSum { get => CurentLanguage.Strings["CreateCashWriteSum"]; }
        public string CreateCashAdd { get => CurentLanguage.Strings["CreateCashAdd"]; }
        public string CreateCashChange { get => CurentLanguage.Strings["CreateCashChange"]; }
        public string ExeptionCreateCashValue { get => CurentLanguage.Strings["ExeptionCreateCashValue"]; }
        public string ExeptionCreateCashDate { get => CurentLanguage.Strings["ExeptionCreateCashDate"]; }
        public string ExeptionCreateCashDescription { get => CurentLanguage.Strings["ExeptionCreateCashDescription"]; }
        public string CreateCashPrice { get => CurentLanguage.Strings["CreateCashPrice"]; }
        public string CreateCashCount { get => CurentLanguage.Strings["CreateCashCount"]; }
        public string CreateCashDescription { get => CurentLanguage.Strings["CreateCashDescription"]; }
        public string CreateCashDescriptionInfo { get => CurentLanguage.Strings["CreateCashDescriptionInfo"]; }

        public string CalendarMon { get => CurentLanguage.Strings["CalendarMon"]; }
        public string CalendarTue { get => CurentLanguage.Strings["CalendarTue"]; }
        public string CalendarWed { get => CurentLanguage.Strings["CalendarWed"]; }
        public string CalendarThu { get => CurentLanguage.Strings["CalendarThu"]; }
        public string CalendarFri { get => CurentLanguage.Strings["CalendarFri"]; }
        public string CalendarSat { get => CurentLanguage.Strings["CalendarSat"]; }
        public string CalendarSun { get => CurentLanguage.Strings["CalendarSun"]; }


        public string HistoryTotalProfit { get => CurentLanguage.Strings["HistoryTotalProfit"]; }
        public string HistoryTotalCosts { get => CurentLanguage.Strings["HistoryTotalCosts"]; }

        public string MainDate { get => CurentLanguage.Strings["MainDate"]; }
        public string MainSum { get => CurentLanguage.Strings["MainSum"]; }
        //
        public string ToolTipCreateCategory { get => CurentLanguage.Strings["ToolTipCreateCategory"]; }
        public string ToolTipCalendar { get => CurentLanguage.Strings["ToolTipCalendar"]; }
        public string ToolTipCreateHistory { get => CurentLanguage.Strings["ToolTipCreateHistory"]; }

        public string MainTotal { get => CurentLanguage.Strings["MainTotal"]; }
        public string Delete { get => CurentLanguage.Strings["Delete"]; }
        public string Update { get => CurentLanguage.Strings["Update"]; }
       
        public string MessageBoxCategory { get => CurentLanguage.Strings["MessageBoxCategory"]; }
        public string MessageBoxCash { get => CurentLanguage.Strings["MessageBoxCash"]; }

        

        #endregion

        #region Libraly
        private Languages GetEngLanguage()
        {
            Languages Eng = new Languages() { Language = "Eng", Strings = new Dictionary<string, string>() };
            Eng.Strings.Add("CategoryProfit", "Profit");
            Eng.Strings.Add("CategoryCosts", "Costs");
            Eng.Strings.Add("CreateCategory", "Create");
            Eng.Strings.Add("СhoseTypeCategory", "Type category");
            Eng.Strings.Add("СhoseNameCategory", "Name");
            Eng.Strings.Add("PriceCategory", "Price");
            Eng.Strings.Add("PriceCategoryInfo", "If you need a static price");
            Eng.Strings.Add("ColorsCategory", "Colors");
            Eng.Strings.Add("ImageCategories", "Image");
            Eng.Strings.Add("YourCategory", "Your category");
            Eng.Strings.Add("ChangeCategory", "Change");
            Eng.Strings.Add("ExeptionNameSmallCategory", "Short name");
            Eng.Strings.Add("ExeptionNameBigCategory", "Long name");
            Eng.Strings.Add("ExeptionNameSymbolCategory", "You can't use special characters");
            Eng.Strings.Add("ExeptionTypeCategory", "Chose type category");

            Eng.Strings.Add("CreateCash", "Create");
            Eng.Strings.Add("CreateCashWriteSum", "Write sum in");
            Eng.Strings.Add("CreateCashAdd", "Add");
            Eng.Strings.Add("CreateCashChange", "Change");
            Eng.Strings.Add("ExeptionCreateCashValue", "Negative values are not allowed");
            Eng.Strings.Add("ExeptionCreateCashDate", "Can't be created for the following days");
            Eng.Strings.Add("ExeptionCreateCashDescription", "You can't use special characters");
            Eng.Strings.Add("CreateCashPrice", "Price");
            Eng.Strings.Add("CreateCashCount", "Count");
            Eng.Strings.Add("CreateCashDescription", "Description");
            Eng.Strings.Add("CreateCashDescriptionInfo", "If you need");

            Eng.Strings.Add("CalendarMon", "Mon");
            Eng.Strings.Add("CalendarTue", "Tue");
            Eng.Strings.Add("CalendarWed", "Wed");
            Eng.Strings.Add("CalendarThu", "Thu");
            Eng.Strings.Add("CalendarFri", "Fri");
            Eng.Strings.Add("CalendarSat", "Sat");
            Eng.Strings.Add("CalendarSun", "Sun");

            Eng.Strings.Add("HistoryTotalProfit", "Total Profit");
            Eng.Strings.Add("HistoryTotalCosts", "Total Costs");

            Eng.Strings.Add("PropertyThame", "Theme");
            Eng.Strings.Add("PropertyImage", "Images");
            Eng.Strings.Add("PropertyImageAdd", "Add new image");
            Eng.Strings.Add("PropertyImageNo", "Do not use");
            Eng.Strings.Add("PropertyLanguage", "Languages");
            Eng.Strings.Add("PropertyDiagram", "Type diagram");
            Eng.Strings.Add("PropertyOpus", "From yourself");
            Eng.Strings.Add("PropertyOpus1", "“Cash Manager” - is a free app to track your expenses. If you like it or you find any problem in the program, please ");
            Eng.Strings.Add("PropertyOpusHyper1", "Click to write me back.");
            Eng.Strings.Add("PropertyOpus2", "The author of the program is “Hi” , and she worked on the design Dana.");
            Eng.Strings.Add("PropertyOpusHyper2", "Click to write to her");

            Eng.Strings.Add("ToolTipCreateCategory", "Create Category");
            Eng.Strings.Add("ToolTipCalendar", "Calendar");
            Eng.Strings.Add("ToolTipCreateHistory", "History");

            Eng.Strings.Add("MainTotal", "Total sum:");
            Eng.Strings.Add("Delete", "Delete");
            Eng.Strings.Add("Update", "Update");

            Eng.Strings.Add("MessageBoxCash", "Do you really want to delete a notation ");
            Eng.Strings.Add("MessageBoxCategory", "Do you really want to delete the category");
            return Eng;
        }

        private Languages GetUkrLanguage()
        {


            Languages UKR = new Languages() { Language = "Ukr", Strings = new Dictionary<string, string>() };
            UKR.Strings.Add("CategoryProfit", "Прибуток");
            UKR.Strings.Add("CategoryCosts", "Витрати");
            UKR.Strings.Add("CreateCategory", "Створити");
            UKR.Strings.Add("СhoseTypeCategory", "Тип категорії");
            UKR.Strings.Add("СhoseNameCategory", "Назва");
            UKR.Strings.Add("PriceCategory", "Ціна");
            UKR.Strings.Add("PriceCategoryInfo", "Якщо бажаєте встановити статичну ціну");
            UKR.Strings.Add("ColorsCategory", "Кольори");
            UKR.Strings.Add("ImageCategories", "Зображення");
            UKR.Strings.Add("YourCategory", "Ваша категорія");
            UKR.Strings.Add("ChangeCategory", "Змінити");
            UKR.Strings.Add("ExeptionNameSmallCategory", "Закоротка назва");
            UKR.Strings.Add("ExeptionNameBigCategory", "Задовга назва");
            UKR.Strings.Add("ExeptionNameSymbolCategory", "Не можна використовувати спеціальні символи");
            UKR.Strings.Add("ExeptionTypeCategory", "Оберіть тип категорії");

            UKR.Strings.Add("CreateCash", "Створити кеш");
            UKR.Strings.Add("CreateCashWriteSum", "Ведіть суму у");
            UKR.Strings.Add("CreateCashAdd", "Добавити");
            UKR.Strings.Add("CreateCashChange", "Змінити");
            UKR.Strings.Add("ExeptionCreateCashValue", "Не можливо створити з міносовим значенням");
            UKR.Strings.Add("ExeptionCreateCashDate", "Не можливо створити на наступні дні");
            UKR.Strings.Add("ExeptionCreateCashDescription", "Не можливо створити з спеціальними символами");
            UKR.Strings.Add("CreateCashPrice", "Ціна");
            UKR.Strings.Add("CreateCashCount", "Кількість");
            UKR.Strings.Add("CreateCashDescription", "Опис");
            UKR.Strings.Add("CreateCashDescriptionInfo", "Якщо потрібно опис");

            UKR.Strings.Add("CalendarMon", "Пн");
            UKR.Strings.Add("CalendarTue", "Вт");
            UKR.Strings.Add("CalendarWed", "Ср");
            UKR.Strings.Add("CalendarThu", "Чт");
            UKR.Strings.Add("CalendarFri", "Пт");
            UKR.Strings.Add("CalendarSat", "Сб");
            UKR.Strings.Add("CalendarSun", "Нд");

            UKR.Strings.Add("HistoryTotalProfit", "Загальний прибуток");
            UKR.Strings.Add("HistoryTotalCosts", "Загальні витрати");

            UKR.Strings.Add("MainDate", "");
            UKR.Strings.Add("MainSum", "Загальні витрати");

            UKR.Strings.Add("PropertyThame", "Теми");
            UKR.Strings.Add("PropertyImage", "Зображення");
            UKR.Strings.Add("PropertyImageAdd", "Добавити нове");
            UKR.Strings.Add("PropertyImageNo", "Не використовувати");
            UKR.Strings.Add("PropertyLanguage", "Мови");
            UKR.Strings.Add("PropertyDiagram", "Тип діаграми");
            UKR.Strings.Add("PropertyOpus", "Від себе");
            UKR.Strings.Add("PropertyOpus1", "“Cash Manager” - це безкоштовна програма для відстеження ваших витрат. Якщо вам вона сподобається, або ви знайдете якусь проблему в програмі, будь ласка, ");
            UKR.Strings.Add("PropertyOpusHyper1", "Нажміть, щоб відписати мені.");
            UKR.Strings.Add("PropertyOpus2", "Автором програми є “Hi”, а над дизайном працювала Дана.");
            UKR.Strings.Add("PropertyOpusHyper2", "Напиисати їй");

            UKR.Strings.Add("ToolTipCreateCategory", "Створити категорію");
            UKR.Strings.Add("ToolTipCalendar", "Календар");
            UKR.Strings.Add("ToolTipCreateHistory", "Історія");

            UKR.Strings.Add("MainTotal", "Загальна сума:");
            UKR.Strings.Add("Delete", "Видалити");
            UKR.Strings.Add("Update", "Змінити");

            UKR.Strings.Add("MessageBoxCash", "Ви дійсно бажаєте видалити запис");
            UKR.Strings.Add("MessageBoxCategory", "Ви дійсно хочете видалити категорію");

            return UKR;
        }
        #endregion


        #region OtherUpdate
        public string GetMaounthName(int monthId, bool isForCalendar)
        {
            if (CurentNameLanguage == "Ukr")
            {
                if (isForCalendar)
                    return MounthUkrCalendar[monthId - 1];
                else
                    return MounthUkr[monthId - 1];
            }
            else
            {
                return MounthEng[monthId - 1];
            }
        }

        public string GetDayOfWeek(int day)
        {

            if (CurentNameLanguage == "Ukr")
            {
                return this.DayOfWeekUkr[day];
            }
            else
            {
                return this.DayOfWeekEng[day];
            }
        }

        #endregion

    }
}
