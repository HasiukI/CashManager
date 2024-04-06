using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CashManager.Model
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isCosts { get; set; }
        public decimal Price { get; set; }
        public string Color {  get; set; }
        public string ImageName { get; set; }
        public BitmapImage Image { get; set; }
        public bool IsActual { get; set; }
       
    }
}
