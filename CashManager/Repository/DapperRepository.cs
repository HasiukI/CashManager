using CashManager.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashManager.Repository
{
    internal class DapperRepository : IRepository
    {
        private string _connectionString = string.Empty;

        public DapperRepository(string connectionString) { 
            _connectionString = connectionString;
        }


        #region Category
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.QueryAsync<Category>("Select * from [Category];");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public IEnumerable<Category> GetAllCategory()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.Query<Category>("Select * from [Category];");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    
                    return await connection.QuerySingleAsync<int>("Insert [Category] OUTPUT INSERTED.Id values (@name,@isCosts,@price);",
                        new
                        {
                            name=category.Name,
                            isCosts= category.isCosts,
                            price= category.Price,
                        });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        #endregion
        #region Cash
        public async Task CreateCash(Cash cash)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.ExecuteAsync("Insert [Cash] values (@catId,@count,@date,@price);",
                        new
                        {
                            catId = cash.CategoryId,
                            count = cash.Count,
                            date = cash.CreatedAt,
                            price = cash.Price,
                        });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
