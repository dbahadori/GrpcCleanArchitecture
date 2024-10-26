using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.DTO.V1.User;
using CleanArchitectureTemplate.Application.UseCases.Implementations.Users;
using CleanArchitectureTemplate.Domain.Common.Validations;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Interfaces.Repositories;
using Moq;

namespace CleanArchitectureTemplate.Application.Tests.UseCases.Implementations.Users
{
    [TestFixture]
    public class AddUserInteractorTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IModelValidator> _modelValidatorMock;
        private AddUserInteractor _addUserInteractor;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _modelValidatorMock = new Mock<IModelValidator>();
            _addUserInteractor = new AddUserInteractor(_unitOfWorkMock.Object, _modelValidatorMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnSuccess_WhenUserIsAddedSuccessfully()
        {
            // Arrange
            var request = new AddUserRequestDTO
            {
                FirstName = "Davood",
                LastName = "Bahadori",
                NationalCode = "1234567890",
                BirthDay = new DateTime(1990, 1, 1)
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>())).ReturnsAsync(new UserEntity());
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _addUserInteractor.ExecuteAsync(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
            _unitOfWorkMock.Verify(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public void ExecuteAsync_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var request = new AddUserRequestDTO
            {
                FirstName = "Davood",
                LastName = "Bahadori",
                NationalCode = "1234567890",
                BirthDay = new DateTime(1990, 1, 1)
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>())).Throws(new FluentValidation.ValidationException("Validation failed"));

            // Act & Assert
            Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _addUserInteractor.ExecuteAsync(request, CancellationToken.None));
            _unitOfWorkMock.Verify(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new AddUserRequestDTO
            {
                FirstName = "John",
                LastName = "Doe",
                NationalCode = "1234567890",
                BirthDay = new DateTime(1990, 1, 1)
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>())).Throws(new EntityCreationException());
            _unitOfWorkMock.Setup(x => x.Rollback());

            // Act

            var result = await _addUserInteractor.ExecuteAsync(request, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccessful, Is.False);
            Assert.That(result.Exception, Is.InstanceOf<EntityCreationException>());

            _unitOfWorkMock.Verify(x => x.UserRepository.AddAsync(It.IsAny<UserEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Rollback(), Times.Once);
 

            
        }
    }
}
