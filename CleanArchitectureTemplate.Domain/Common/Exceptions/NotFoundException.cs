
namespace CleanArchitectureTemplate.Domain.Common.Exceptions;

public class NotFoundException : CustomException
{

    public NotFoundException()
        : base(string.Empty, null)
    {
    }

  
    public NotFoundException(string resourceName, object key, string? userFriendlyMessage)
        : base(userFriendlyMessage)
    {
        DeveloperDetail = $"Resource \"{resourceName}\" ({key}) was not found.";
    }
}
