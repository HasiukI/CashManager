using CashManager.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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
                    
                    return await connection.QuerySingleAsync<int>("Insert [Category] OUTPUT INSERTED.Id values (@name,@isCosts,@price,@color,@fileName,@isActual);",
                        new
                        {
                            name=category.Name,
                            isCosts= category.isCosts,
                            price= category.Price,
                            color = category.Color,
                            fileName = category.ImageName,
                            isActual = category.IsActual,
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
                    await connection.ExecuteAsync("Insert [Cash] values (@catId,@date,@price);",
                        new
                        {
                            catId = cash.CategoryId,
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

        #region History
        /// <summary>
        /// Search in DB all cash and category
        /// </summary>
        /// <returns>IEnumerable<History></returns>
        public IEnumerable<History> LoadHistory(DateTime date)
        {

            List<History> histories = new List<History>();
            try
            {
                IEnumerable<Cash> cashes = null;
                using (var connection = new SqlConnection(_connectionString))
                {
                    cashes = connection.Query<Cash>("Select * from [Cash] where MONTH([CreatedAt]) =@mounth and YEAR([CreatedAt]) =@year;",
                        new
                        {
                            mounth = date.Month,
                            year = date.Year,
                        });

                    foreach(Cash cash in cashes)
                    {
                        Category category = connection.QueryFirst<Category>("Select * from [Category] where [Category].[Id] = @Id", new { Id = cash.CategoryId });
                        category.Image = ReadPicture(category.ImageName);

                        histories.Add(new History()
                        {
                            Cash = cash,
                            Category = category
                        });
                    }

                    return histories;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        #endregion

        #region Other
        private BitmapImage ReadPicture(string nameImage)
        {
            DirectoryInfo dir = new DirectoryInfo(GetRootDirectoryImages());
            return new BitmapImage(new Uri($"{dir.FullName}//{nameImage}", UriKind.Absolute));
        }

        private string GetRootDirectoryImages()
        {
            string dirrr = Directory.GetCurrentDirectory();
            return dirrr.Remove(dirrr.IndexOf("bin")) + "Images";
        }
        #endregion
    }
}
