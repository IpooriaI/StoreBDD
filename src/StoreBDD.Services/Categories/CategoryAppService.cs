using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using System.Collections.Generic;

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

            CheckIfTitleIsDuplicate(dto.Title);

            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = GetCategory(id);

            if (category.Products != null)
            {
                throw new CategoryHasProductsException();
            }

            _repository.Delete(category);
            _unitOfWork.Commit();
        }


        public GetCategoryDto Get(int id)
        {
            return _repository.Get(id);
        }

        public List<GetCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            var category = GetCategory(id);
            CheckIfTitleIsDuplicate(dto.Title, id);

            category.Title = dto.Title;

            _unitOfWork.Commit();
        }

        private Category GetCategory(int id)
        {
            var category = _repository.GetById(id);

            if (category == null)
            {
                throw new CategoryNotFoundException();
            }

            return category;
        }

        private void CheckIfTitleIsDuplicate(string Title, int ignoreId = 0)
        {
            if (_repository.CheckTitle(Title, ignoreId))
            {
                throw new DuplicateCategoryTitleException();
            }
        }
    }
}
