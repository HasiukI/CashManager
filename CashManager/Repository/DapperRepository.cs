using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Repository
{
    internal class DapperRepository
    {
        private readonly string _sqlConnection = String.Empty;

        public DapperRepository(string sqlConnection) { 
            _sqlConnection = sqlConnection;
        }


    }
}
