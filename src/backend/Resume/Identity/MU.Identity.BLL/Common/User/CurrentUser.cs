using MU.Identity.BLL.Exceptions;

namespace MU.Identity.BLL.Common.User;

public class CurrentUser : ICurrentUser
{
    private Guid _id;

    public Guid Id
    {
        get { return _id; }
    }

    public CurrentUser(IAccessTokenProvider accessor)
    {
        var user = accessor.GetUser();

        if (!Guid.TryParse(user.Identity.Name, out _id))
        {
            throw new UserRecognitionException($"User {user.Identity.Name} is not a valid user id.");
        }
    }
}

public interface ICurrentUser
{
    public Guid Id { get; }
}
