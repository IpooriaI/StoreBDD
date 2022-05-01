using StoreBDD.Entities;
using StoreBDD.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool CheckTitle(string title)
        {
            return _dataContext.Categories.Any(_ => _.Title == title);
        }

        public Category GetById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(c => c.Id == id);
        }
    }
}
