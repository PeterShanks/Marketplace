using Marketplace.EventSourcing;
using Marketplace.Users.Domain.UserProfiles;
using Marketplace.Users.Messages;
using Marketplace.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Marketplace.Users.UserProfiles;

[Route("api/profile")]
public class UserProfileCommandsApi : CommandApi<UserProfile>
{
    public UserProfileCommandsApi(
        ILogger<UserProfileCommandsApi> logger,
        UserProfileCommandService applicationService)
        : base(logger, applicationService)
    {
    }

    [HttpPost]
    public Task<IActionResult> Post(Commands.V1.RegisterUser request)
    {
        return HandleCommand(request);
    }

    [HttpPut("fullname")]
    public Task<IActionResult> Put(Commands.V1.UpdateUserFullName request)
    {
        return HandleCommand(request);
    }

    [HttpPut("displayname")]
    public Task<IActionResult> Put(Commands.V1.UpdateUserDisplayName request)
    {
        return HandleCommand(request);
    }

    [HttpPut("photo")]
    public Task<IActionResult> Put(Commands.V1.UpdateUserProfilePhoto request)
    {
        return HandleCommand(request);
    }
}