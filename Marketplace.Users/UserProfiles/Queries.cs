using Marketplace.Users.Auth;
using static Marketplace.Users.Projections.ReadModels;

namespace Marketplace.Users.UserProfiles;

public static class Queries
{
    public static async Task<UserDetails?> GetUserDetails(
        this GetUsersModuleSession getSession,
        Guid id
    )
    {
       using var session = getSession();

       var userDetails = await session.LoadAsync<UserDetails?>(UserDetails.GetDatabaseId(id));

       return userDetails;
    }
}