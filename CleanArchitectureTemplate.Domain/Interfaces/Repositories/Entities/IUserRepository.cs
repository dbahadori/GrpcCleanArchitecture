using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObejects;
using Ticketing.Domain.Interfaces.Repositories;


namespace CleanArchitectureTemplate.Domain.Interfaces.Repositories.Entities
{
    public interface IUserRepository :
        IReadableRepository<UserEntity, Guid>,
        IWritableRepository<UserEntity, Guid>,
        IPaginatedRepository<UserEntity>,
        IExistenceRepository<UserEntity, Guid>
    {
        Task<UserEntity?> FindByNationalCodeAsync(string email);
    }
}
