using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Entities
{
    public class Category
    {
        public string Title { get; set; }
        public List<Product> Products { get; set; }
    }
}
