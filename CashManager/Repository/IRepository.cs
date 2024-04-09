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
        #region Category
        public IEnumerable<Category> LoadCategories();
        public Task DeleteCategoryAsync(Category category);
        #endregion
        #region Cash
        public Task CreateCashAsync(Cash cash);
        #endregion
        #region History
        public IEnumerable<History> LoadHistory(DateTime date);
        #endregion
        #region Other
        public List<BitmapImage> ReadAllStaticPictures();
        public string GetRootDirectoryImages();
        #endregion
    }
}
