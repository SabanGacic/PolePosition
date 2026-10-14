namespace PolePosition.API.Common;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}