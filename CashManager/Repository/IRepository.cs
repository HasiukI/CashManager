using CashManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
