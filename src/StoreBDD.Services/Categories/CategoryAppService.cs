using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _repository;
        public CategoryAppService(CategoryRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            var category = new Category
            {
                Title = dto.Title,
            };

            var checkTitle = _repository.CheckTitle(dto.Title);

            if (checkTitle)
            {
                throw new DuplicateCategoryTitleException();
            }

            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = _repository.GetById(id);

            if (category == null)
            {
                throw new CategoryNotFoundException();
            }
            if (category.Products != null)
            {
                throw new CategoryHasProductsException();
            }
            _repository.Delete(category);
            _unitOfWork.Commit();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            var category = _repository.GetById(id);
            var checkTitle = _repository.CheckTitle(dto.Title);

            if (category == null)
            {
                throw new CategoryNotFoundException();
            }

            if (checkTitle)
            {
                throw new DuplicateCategoryTitleException();
            }

            category.Title = dto.Title;

            _unitOfWork.Commit();
        }
    }
}
