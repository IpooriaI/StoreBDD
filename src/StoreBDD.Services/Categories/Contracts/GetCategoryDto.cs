using StoreBDD.Services.Products.Contracts;
using System.Collections.Generic;

namespace StoreBDD.Services.Categories.Contracts
{
    public class GetCategoryDto
    {
        public string Title { get; set; }
        public List<GetProductDto> Products { get; set; }
    }
}
