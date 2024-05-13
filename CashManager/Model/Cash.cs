using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Model
{
    internal class Cash
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
