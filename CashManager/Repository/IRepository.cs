using CashManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CashManager.Repository
{
     interface IRepository
    {
        #region Diagram
        public IEnumerable<AllCashInCategory> GetDiagram(DateTime date, List<Category> curentCategories);
        #endregion

        #region Category
        public IEnumerable<Category> GetAllCategory();
        public Task CreateCategoryAsync(Category category);
        public Task UpdateOrDeleteCategoryAsync(List<Category> curentCutegorie);
        #endregion

        #region Cash
        public Task CreateCashAsync(Cash cash);
        public Task DeleteCashAsync(Cash cash);
        public Task UpdateCashAsync(Cash cash);
        #endregion

        #region History
        public IEnumerable<HistoryDay> GetAllHistoryInMounth(DateTime date, List<Category> curentCategories);
        #endregion

        #region Other
        public List<BitmapImage> ReadAllStaticPictures();
        public string GetRootDirectory(string nameDir);
        public IEnumerable<BitmapImage> GetAllBackGrounds();
        public Task<BitmapImage> AddBackGround(string fileName,int lastId);
        public Task DeleteBackGround(string fileName);
        #endregion

        #region MainInfo
        public MainInfo GetMainInfo();
        public Task UpdateMainInfo(MainInfo newMainInfo);
        #endregion


    }
}
