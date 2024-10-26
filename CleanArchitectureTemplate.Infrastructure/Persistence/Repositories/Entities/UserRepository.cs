using AutoMapper;
using CleanArchitectureTemplate.Domain.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories.Entities;
using CleanArchitectureTemplate.Infrastructure.Persistence.Context;
using CleanArchitectureTemplate.Resources;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories.Entities
{
    public class UserRepository : BaseRepository<UserEntity, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<UserEntity?> FindByNationalCodeAsync(string nationalCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nationalCode))
                    throw new ArgumentNullException();

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.NationalCode!.ToUpper() == nationalCode.ToUpper());


                return user;
            }
            catch (Exception exception)
            {
                var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.ErrorDuringUserRetrievalByEmail);
                 throw new RepositoryException()
                    .WithUserFriendlyMessage(localizedMessage)
                    .WithDeveloperDetail(defaultMessage)
                    .WithInnerCustomException(exception);
            }
            
        }

    }
}
