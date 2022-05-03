using StoreBDD.Entities;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Products.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace StoreBDD.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;

        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public bool CheckTitle(string title,int ignoreId)
        {
            return _dataContext.Categories
                .Where(_ => _.Id != ignoreId)
                .Any(_ => _.Title == title);
        }

        public void Delete(Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public GetCategoryDto Get(int id)
        {
            return _dataContext.Categories
                .Where(_ => _.Id == id)
                .Select(_ => new GetCategoryDto
                {
                    Title = _.Title,
                    Products = _.Products.Select(_ => new GetProductDto
                    {
                        Name = _.Name,
                        Count = _.Count,
                        Price = _.Price,
                        MinimumCount = _.MinimumCount,
                        CategoryId = _.CategoryId,
                    }).ToList()
                }).FirstOrDefault();
        }

        public List<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories.Select(_ => new GetCategoryDto
            {
                Title = _.Title,
                Products = _.Products.Select(_ => new GetProductDto
                {
                    Name = _.Name,
                    Count = _.Count,
                    Price = _.Price,
                    MinimumCount = _.MinimumCount,
                    CategoryId = _.CategoryId,
                }).ToList(),

            }).ToList();
        }

        public Category GetById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(c => c.Id == id);
        }
    }
}
