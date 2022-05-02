using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;
using System.Linq;

namespace StoreBDD.Persistence.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _dataContext;

        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Product product)
        {
            _dataContext.Products.Add(product);
        }

        public bool CheckName(int categoryId, string name)
        {
            return _dataContext.Products
                .Where(_ => _.CategoryId == categoryId)
                .Any(_ => _.Name == name);
        }

        public void Delete(Product product)
        {
            _dataContext.Products.Remove(product);
        }

        public Product GetById(int id)
        {
            return _dataContext.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
