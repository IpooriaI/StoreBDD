using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Entities
{
    public class SellFactor
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime DateSold { get; set; }
    }
}
