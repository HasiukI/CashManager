using CashManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using System.Data.SqlClient;

namespace CashManager.Repository
{
    internal class DapperRepository : IRepository
    {
        private readonly string _sqlConnection = String.Empty;

        public DapperRepository(string sqlConnection) { 
            _sqlConnection = sqlConnection;
        }

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            try
            {
                using(var connection  = new SqlConnection(_sqlConnection))
                {
                    return await connection.QueryAsync<Category>("Select * from [Category];");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
