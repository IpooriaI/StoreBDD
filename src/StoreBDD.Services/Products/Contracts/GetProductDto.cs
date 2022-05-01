using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.Products.Contracts
{
    public class GetProductDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int MinimumCount { get; set; }
        public int CategoryId { get; set; }
    }
}
