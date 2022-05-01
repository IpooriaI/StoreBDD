using StoreBDD.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.Categories.Contracts
{
    public class GetCategoryDto
    {
        public string Title { get; set; }
        public List<GetProductDto> Products { get; set; }
    }
}
