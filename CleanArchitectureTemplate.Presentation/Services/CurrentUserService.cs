using CleanArchitectureTemplate.Application.Common.Interfaces;
using System.Security.Claims;

namespace CleanArchitectureTemplate.Presentation.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        
        public CurrentUserService()
        {
            
        }
        private string? _userId;

        public string? UserId
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

    }
}