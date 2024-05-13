using CashManager.Model;
using CashManager.Serialazer;
using CashManager.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CashManager.Repository
{
    class FileRepository : IRepository
    {
        private readonly FileSerializer serializer = null;
        private readonly List<BitmapImage> _staticImages = null;

        public List<HistoryDay> allHistoryInMounth = null;


        public FileRepository() { 
            serializer = new FileSerializer();
            _staticImages = ReadAllStaticPictures();
        }


        #region Category
       
        public IEnumerable<Category> GetAllCategory()
        {
            List<Category> categories = new List<Category>();
            string path = $"{GetRootDirectory("Files")}\\Categories.bit";

            using (FileStream fl = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                {
                    while (sr.Peek() > 0)
                    {
                        string str = sr.ReadLine();
                        if (str != null && !String.IsNullOrEmpty(str))
                        {
                            Category category = serializer.DesialazeFile<Category>(str);
                            category.Image = _staticImages.Where(i => i.UriSource.Segments.Last() == category.ImageName).First();
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

        public async Task UpdateOrDeleteCategoryAsync(List<Category> curentCutegorie)
        {

            SetNullFileCategory();

            foreach (Category cat in curentCutegorie)
            {
                await CreateCategoryAsync(cat);
            }
        }

        public async Task CreateCategoryAsync(Category category)
        {
            string path = $"{GetRootDirectory("Files")}\\Categories.bit";

            using (FileStream fl = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
                {
                    string str = serializer.SerialazerFile<Category>(category);
                    await sr.WriteLineAsync(str);
                }
            }
        }

        private async Task SetNullFileCategory()
        {
            string path = $"{GetRootDirectory("Files")}\\Categories.bit";
            await File.WriteAllTextAsync(path, string.Empty);
        }

        #endregion

        #region Cash

        public async Task CreateCashAsync(Cash cash)
        {
            try
            {
                if (cash == null)
                    throw new ArgumentNullException(nameof(cash));

                string pathFile = $"{GetRootDirectory("Files")}\\Cashes\\{cash.CreatedAt.Month}_{cash.CreatedAt.Year}.bit";

                if (!File.Exists(pathFile))
                {
                    using (FileStream fs = File.Create(pathFile)) { }
                }

                using (FileStream fl = new FileStream(pathFile, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
                    {
                        string str = serializer.SerialazerFile<Cash>(cash);
                        await sr.WriteLineAsync(str);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteCashAsync(Cash cash)
        {
            List<Cash> cashes = LoadCashes(cash.CreatedAt).ToList();

            await SetNullCashFile(cash.CreatedAt);

            foreach (Cash c in cashes)
            {
                if(c.Id != cash.Id)
                {
                    await CreateCashAsync(c);
                }
            }
        }

        public async Task UpdateCashAsync(Cash cash)
        {
            List<Cash> cashes = LoadCashes(cash.CreatedAt).ToList();

            await SetNullCashFile(cash.CreatedAt);

            foreach (Cash c in cashes)
            {
                if (c.Id != cash.Id)
                {
                    await CreateCashAsync(c);
                }
                else
                {
                    await CreateCashAsync(cash);
                }
            }
        }

        public IEnumerable<Cash> LoadCashes(DateTime date)
        {
            try
            {
                if (date == null)
                    throw new Exception("Dont found date.");

                List<Cash> cashes = new List<Cash>();
                string pathFile = $"{GetRootDirectory("Files")}\\Cashes\\{date.Month}_{date.Year}.bit";

                if (!File.Exists(pathFile))
                {
                    return cashes;
                }

                using (FileStream fl = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                    {
                        while (sr.Peek() > 0)
                        {
                            string str = sr.ReadLine();
                            if (str != null && !String.IsNullOrEmpty(str))
                            {
                                Cash cash = serializer.DesialazeFile<Cash>(str);
                                cashes.Add(cash);
                            }
                        }
                    }
                }
                return cashes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        private async Task SetNullCashFile(DateTime date)
        {
            string pathFile = $"{GetRootDirectory("Files")}\\Cashes\\{date.Month}_{date.Year}.bit";
            await File.WriteAllTextAsync(pathFile, string.Empty);
        }
        #endregion

        #region History

        public IEnumerable<HistoryDay> GetAllHistoryInMounth(DateTime date, List<Category> curentCutegories)
        {
            List<HistoryDay> histories = new List<HistoryDay>();

            for (int i = 0; i < (int)(new DateTime(date.Year, date.Month, 1).DayOfWeek) - 1; i++)
            {
                histories.Add(null);
            }

            var cashes = LoadCashes(date).ToList();

            if (cashes.Count == 0 || curentCutegories.Count == 0)
            {
                for (int i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
                {
                    histories.Add(new HistoryDay() { Date = new DateTime(date.Year, date.Month, i + 1) });
                }

                return histories;
            }

            cashes = cashes.OrderBy(c => c.CreatedAt).ToList();


            for (int i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                HistoryDay day = new HistoryDay()
                {
                    Date = new DateTime(date.Year, date.Month, i + 1),
                    HistoriesInDay = new ObservableCollection<AllInfoForCash>()
                };

                List<Cash> dayCash = cashes.Where(c => c.CreatedAt.Day == i + 1).ToList();

                decimal profit = 0;
                decimal costs = 0;

                foreach (Cash c in dayCash)
                {
                    Category categoryForCash = curentCutegories.Where(cat => cat.Id == c.CategoryId).FirstOrDefault();

                    if (categoryForCash == null)
                        throw new Exception("Dont find Category to Cash. LoadHistory().FileRepository");

                    if (categoryForCash.isCosts)
                        costs += c.Price;
                    else
                        profit += c.Price;

                    day.HistoriesInDay.Add(new AllInfoForCash() { Cash = c, Category = categoryForCash });
                }

                day.TotalProfit = profit;
                day.TotalCosts = costs;
                histories.Add(day);
            }

            return histories;
        }

        #endregion

        #region Diagram

        public IEnumerable<AllCashInCategory> GetDiagram(DateTime date, List<Category> curentCategories)
        {
            List<AllCashInCategory> diagram = new List<AllCashInCategory>();

            try
            {

                List<Cash> cashes = LoadCashes(date).ToList();

                foreach (Cash cash in cashes)
                {
                    Category category = curentCategories.Where(c => c.Id == cash.CategoryId).FirstOrDefault();

                    if (category != null)
                    {
                        AllCashInCategory allCashInCategory = diagram.Where(d => d.Category == category).FirstOrDefault();

                        if (allCashInCategory == null)
                        {
                            allCashInCategory = new AllCashInCategory()
                            {
                                Category = category,
                                Cashes = new ObservableCollection<Cash>(),
                                TotalSum = 0
                            };
                            diagram.Add(allCashInCategory);

                        }
                        allCashInCategory.Cashes.Add(cash);
                        allCashInCategory.TotalSum += cash.Price;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption For: FileRepository in GetDiagram\n" + ex.Message);
            }

            return diagram;
        }
        #endregion

        #region MainInfo
        public MainInfo GetMainInfo()
        {
            string pathFile = $"{GetRootDirectory("Files")}\\MainInfo.bit";
            MainInfo mainInfo = null;

            try
            {

                if (!File.Exists(pathFile))
                {
                    using (FileStream fs = File.Create(pathFile))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                        {
                            sw.WriteLine(serializer.SerialazerFile<MainInfo>(new MainInfo() { LastIdCategory = 0, TotalSum = 0 }));
                        }
                    }
                }

                using (FileStream fl = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                    {
                        while (sr.Peek() > 0)
                        {
                            string str = sr.ReadLine();
                            if (str != null && !String.IsNullOrEmpty(str))
                            {
                                mainInfo = serializer.DesialazeFile<MainInfo>(str);
                            }
                        }
                    }
                }
                return mainInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption FileRepository in GetMainInfo\n" + ex.Message);
                return null;
            }
        }

        public async Task UpdateMainInfo(MainInfo newMainInfo)
        {
            string pathFile = $"{GetRootDirectory("Files")}\\MainInfo.bit";
            try
            {
                using (FileStream fl = new FileStream(pathFile, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fl, Encoding.Unicode))
                    {
                        sw.WriteLine(serializer.SerialazerFile<MainInfo>(newMainInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption FileRepository in GetMainInfo\n" + ex.Message);
            }
        }
        #endregion

        #region Other
        public List<BitmapImage> ReadAllStaticPictures()
        {
            List<BitmapImage> images = new List<BitmapImage>();

            DirectoryInfo dir = new DirectoryInfo(GetRootDirectory("Images\\CategoryImages"));
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                images.Add(new BitmapImage(new Uri(file.FullName, UriKind.Absolute)));
            }

            return images;
        }

        public string GetRootDirectory(string nameDir)
        {
            string dirrr = Directory.GetCurrentDirectory();
            return dirrr.Remove(dirrr.IndexOf("bin")) + $"\\{nameDir}";
        }

        public IEnumerable<BitmapImage> GetAllBackGrounds()
        {
            List<BitmapImage> images = new List<BitmapImage>();

            DirectoryInfo dir = new DirectoryInfo(GetRootDirectory("Images\\backgrounds"));
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(file.FullName, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                images.Add(bitmap);
            }

            return images;
        }

        public async Task<BitmapImage> AddBackGround(string fileName, int lastId)
        {
            string destinationPath = GetRootDirectory("Images\\backgrounds");
            string destFile = System.IO.Path.Combine(destinationPath, $"back{lastId}.{fileName.Split('.').Last()}");
            File.Copy(fileName, destFile, true);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(destFile);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public async Task DeleteBackGround(string fileName)
        {
            if (File.Exists(fileName))
            {
                await Task.Run(() => File.Delete(fileName));

            }
        }
        #endregion
    }
}
