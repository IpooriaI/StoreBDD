using StoreBDD.Infrastructure.Application;

namespace StoreBDD.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dataContext;
        public EFUnitOfWork(EFDataContext dataConext)
        {
            _dataContext = dataConext;
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
        }
    }
}
