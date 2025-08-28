namespace MU.CV.BLL.Exceptions;

public class IntrospectionException : Exception
{
    public IntrospectionException()
    {
    }

    public IntrospectionException(string message) : base(message)
    {
    }

    public IntrospectionException(string message, Exception inner) : base(message, inner)
    {
    }
}