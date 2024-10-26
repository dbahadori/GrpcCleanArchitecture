using CleanArchitectureTemplate.Domain.Interfaces.Repositories.Entities;

namespace CleanArchitectureTemplate.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void DetachRetrievedEntities(IEnumerable<object> entities);

        void DetachAllEntities();
        Task CommitAsync();
        void Commit();

        void Rollback();

        IUserRepository UserRepository { get; }

    }
}
