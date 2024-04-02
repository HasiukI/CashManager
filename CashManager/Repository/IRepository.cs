using CashManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Repository
{
    internal interface IRepository
    {
        #region Category
        Task<IEnumerable<Category>> GetAllCategory();
        #endregion
    }
}
