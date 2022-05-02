using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        void Add(AddCategoryDto dto);
        void Update(int id, UpdateCategoryDto dto);
        void Delete(int id);
        GetCategoryDto Get(int id);
        List<GetCategoryDto> GetAll();
    }
}
