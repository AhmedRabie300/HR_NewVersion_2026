
namespace Application.Common;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}

public class RequiredFieldException : Exception
{
    public RequiredFieldException(string message) : base(message) { }
}