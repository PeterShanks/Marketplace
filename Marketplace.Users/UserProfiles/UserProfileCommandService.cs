using Marketplace.EventSourcing;
using Marketplace.Users.Domain.Shared;
using Marketplace.Users.Domain.UserProfiles;
using Microsoft.Extensions.Logging;
using static Marketplace.Users.Messages.Commands;

namespace Marketplace.Users.UserProfiles;

public class UserProfileCommandService : ApplicationService<UserProfile>
{
    public UserProfileCommandService(
        IAggregateStore store,
        ILogger<UserProfileCommandService> logger,
        CheckTextForProfanity checkText)
        : base(store, logger)
    {
        CreateWhen<V1.RegisterUser>(
            cmd => new UserId(cmd.UserId),
            (cmd, id) => UserProfile.Create(
                new UserId(id),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, checkText)
            )
        );

        UpdateWhen<V1.UpdateUserDisplayName>(
            cmd => new UserId(cmd.UserId),
            (userProfile, cmd) => userProfile.UpdateDisplayName(
                DisplayName.FromString(cmd.DisplayName, checkText)
            )
        );

        UpdateWhen<V1.UpdateUserFullName>(
            cmd => new UserId(cmd.UserId),
            (userProfile, cmd) => userProfile.UpdateFullName(FullName.FromString(cmd.FullName))
        );

        UpdateWhen<V1.UpdateUserProfilePhoto>(
            cmd => new UserId(cmd.UserId),
            (user, cmd) => user.UpdateProfilePhoto(new Uri(cmd.PhotoUrl))
        );
    }
}