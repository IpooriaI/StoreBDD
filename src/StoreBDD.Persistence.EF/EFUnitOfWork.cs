using StoreBDD.Infrastructure.Application;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dataContext;
        public EFUnitOfWork(EFDataContext dataConext)
        {
            _dataContext = dataConext;
        }

        public async Task Commit()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
