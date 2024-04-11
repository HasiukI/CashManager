using CashManager.Model;
using CashManager.Serialazer;
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
        private readonly FileSerializer serializer = null;
        private readonly List<BitmapImage> _staticImages = null;

        public FileRepository() { 
            serializer = new FileSerializer();
            _staticImages = ReadAllStaticPictures();
        }


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
            List<Category> categories = new List<Category>();
            string path = $"{GetRootDirectoryFiles()}\\Categories.bit";

            using (FileStream fl = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                {
                    while (sr.Peek() > 0){
                        string str = sr.ReadLine();
                        if (str != null && !String.IsNullOrEmpty(str))
                        {
                            Category category = serializer.DesialazeFile<Category>(str);
                            category.Image = _staticImages.Where(i => Path.GetFileName(i.UriSource.ToString()) == category.ImageName).First();
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;

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
        public string GetRootDirectoryFiles()
        {
            string dirrr = Directory.GetCurrentDirectory();
            return dirrr.Remove(dirrr.IndexOf("bin")) + "Files";
        }

        public async Task CreateCategoryAsync(Category category)
        {
            string path = $"{GetRootDirectoryFiles()}/Categories.bit";

            using (FileStream fl = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
                {
                    string str = serializer.SerialazerFile<Category>(category);
                    await sr.WriteLineAsync(str);
                }
            }
        }
        #endregion

    }
}
