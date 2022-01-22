using Marketplace.Users.UserProfiles;

namespace Marketplace.Users.Auth;

public class AuthService
{
    private readonly GetUsersModuleSession _getSession;

    public AuthService(GetUsersModuleSession getSession)
    {
        _getSession = getSession;
    }

    public async Task<bool> CheckCredentials(
        string username,
        string password
    )
    {
        var userDetails = await _getSession.GetUserDetails(Guid.Parse(password));

        return userDetails?.DisplayName == username;
    }
}