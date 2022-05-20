using System.Threading.Tasks;

namespace StoreBDD.Infrastructure.Application
{
    public interface UnitOfWork
    {
        Task Commit();
    }
}
