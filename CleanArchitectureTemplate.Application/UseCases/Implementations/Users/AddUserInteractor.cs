using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.Common.Validations;
using CleanArchitectureTemplate.Domain.DTO;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using CleanArchitectureTemplate.Resources;
using FluentValidation;

namespace CleanArchitectureTemplate.Application.UseCases.Implementations.Users
{
    public class AddUserInteractor : IAddUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModelValidator _modelValidator;


        public AddUserInteractor(IUnitOfWork unitOfWork, IModelValidator modelValidator )
        {
            _unitOfWork = unitOfWork;
            _modelValidator = modelValidator;

        }
        public async Task<OperationResult> ExecuteAsync(AddUserRequestDTO request, CancellationToken cancellationToken)
        {
            try 
            {
                // or we can use mapper to map request (dto) to user entity
                var user = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    NationalCode = request.NationalCode,
                    BirthDay = request.BirthDay
                };

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                return OperationResult.Success();
            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch (Exception exception)
            {

                _unitOfWork.Rollback();

                var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(em => ErrorMessages.ErrorDuringAddingEntity, typeof(UserEntity), "NationalCode", request.NationalCode);

                return OperationResult.Failure(
                    new EntityCreationException()
                    .WithMessage(defaultMessage, localizedMessage)
                    .WithInnerCustomException(exception)
                );
            }


        }
    }
}
