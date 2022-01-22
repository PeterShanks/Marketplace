using Marketplace.Users.Auth;
using Marketplace.Users.Projections;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.UserProfiles;

[ApiController]
[Route("api/profile")]
public class UserProfileQueryApi : ControllerBase
{
    private readonly GetUsersModuleSession _getSession;

    public UserProfileQueryApi(GetUsersModuleSession getSession)
    {
        _getSession = getSession;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<ReadModels.UserDetails>> Get(Guid userId)
    {
        var user = await _getSession.GetUserDetails(userId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}