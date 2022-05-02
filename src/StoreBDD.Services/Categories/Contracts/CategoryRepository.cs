using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool CheckTitle(string title);
        Category GetById(int id);
        void Delete(Category category);
        List<GetCategoryDto> GetAll();
        GetCategoryDto Get(int id);
    }
}
