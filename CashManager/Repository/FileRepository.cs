using CashManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CashManager.Repository
{
    class FileRepository : IRepository
    {
        public Task CreateCashAsync(Cash cash)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> LoadCategories()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<History> LoadHistory(DateTime date)
        {
            throw new NotImplementedException();
        }

        #region Other
        public List<BitmapImage> ReadAllStaticPictures()
        {
            List<BitmapImage> images = new List<BitmapImage>();

            DirectoryInfo dir = new DirectoryInfo(GetRootDirectoryImages());
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                images.Add(new BitmapImage(new Uri(file.FullName, UriKind.Absolute)));
            }

            return images;
        }

        /// <summary>
        /// Create Image for Category. In File
        /// </summary>
        /// <param name="nameImage"></param>
        /// <returns>BitmapImage</returns>
        private BitmapImage ReadPicture(string nameImage)
        {
            DirectoryInfo dir = new DirectoryInfo(GetRootDirectoryImages());
            return new BitmapImage(new Uri($"{dir.FullName}//{nameImage}", UriKind.Absolute));
        }

        /// <summary>
        /// Serch Directory with Images
        /// </summary>
        /// <returns>Directory name</returns>
        public string GetRootDirectoryImages()
        {
            string dirrr = Directory.GetCurrentDirectory();
            return dirrr.Remove(dirrr.IndexOf("bin")) + "Images";
        }
        #endregion
    }
}
