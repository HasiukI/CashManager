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
        public Task<IEnumerable<Category>> GetAllCategoryAsync();
        public IEnumerable<Category> GetAllCategory();
        #endregion
    }
}
