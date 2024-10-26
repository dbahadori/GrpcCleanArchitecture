using AutoMapper;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Interfaces.Users;
using CleanArchitectureTemplate.Domain.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Common.Validations;
using CleanArchitectureTemplate.Domain.DTO;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using CleanArchitectureTemplate.Resources;

namespace CleanArchitectureTemplate.Application.UseCases.Implementations.Users
{
    public class UpdateUserInteractor : IUpdateUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModelValidator _modelValidator;
        private readonly IMapper _mapper;
        public UpdateUserInteractor(
            IUnitOfWork unitOfWork,
            IModelValidator modelValidator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _modelValidator = modelValidator;
            _mapper = mapper;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateUserRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

                if (existingUser != null)
                {
                    existingUser.FirstName = !string.IsNullOrEmpty(request.FirstName) ? request.FirstName : existingUser.FirstName;
                    existingUser.LastName = !string.IsNullOrEmpty(request.LastName) ? request.LastName : existingUser.LastName;
                    existingUser.NationalCode = !string.IsNullOrEmpty(request.NationalCode) ? request.NationalCode : existingUser.NationalCode;
                    existingUser.BirthDay = request.BirthDay != default ? request.BirthDay : existingUser.BirthDay;
                    var updatedUser = _unitOfWork.UserRepository.Update(existingUser);
                    await _unitOfWork.CommitAsync();

                    var changeResult = _mapper.Map<UserResponseDTO>(updatedUser);

                    return OperationResult<UserResponseDTO>.Success(changeResult);
                }
                else
                {
                    var (defaultMessage, localizedMessage) = ResourceHelper.GetGeneralErrorMessages(
                        em => ErrorMessages.EntityNotFound, typeof(UserEntity).Name, "Id", request.Id
                        );

                    return OperationResult.Failure(
                        new EntityNotFoundException()
                        .WithDeveloperDetail(defaultMessage)
                        .WithUserFriendlyMessage(localizedMessage)
                        );
                }

            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
