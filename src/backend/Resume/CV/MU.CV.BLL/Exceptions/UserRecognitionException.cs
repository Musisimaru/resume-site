namespace MU.CV.BLL.Exceptions;

public class UserRecognitionException : Exception
{
    public UserRecognitionException()
    {
    }

    public UserRecognitionException(string message) : base(message)
    {
    }

    public UserRecognitionException(string message, Exception inner) : base(message, inner)
    {
    }
}