using Marketplace.EventSourcing;
using static Marketplace.Users.Messages.Events;

namespace Marketplace.Users.Domain.UserProfiles;

public class UserProfile : AggregateRoot
{
    private FullName FullName { get; set; }
    private DisplayName DisplayName { get; set; }
    private string PhotoUrl { get; set; }

    public static UserProfile Create(
        UserId id,
        FullName fullName,
        DisplayName displayName)
    {
        var profile = new UserProfile();

        profile.Apply(new V1.UserRegistered(id, fullName, displayName));

        return profile;
    }

    public void UpdateFullName(FullName fullName)
    {
        Apply(new V1.UserFullNameUpdated(Id, fullName));
    }

    public void UpdateDisplayName(DisplayName displayName)
    {
        Apply(new V1.UserDisplayNameUpdated(Id, displayName));
    }

    public void UpdateProfilePhoto(Uri photoUrl)
    {
        Apply(new V1.ProfilePhotoUploaded(Id, photoUrl.ToString()));
    }

    protected override void EnsureValidState()
    {
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case V1.UserRegistered e:
                Id = new UserId(e.UserId);
                FullName = new FullName(e.FullName);
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case V1.UserFullNameUpdated e:
                FullName = new FullName(e.FullName);
                break;
            case V1.UserDisplayNameUpdated e:
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case V1.ProfilePhotoUploaded e:
                PhotoUrl = e.PhotoUrl;
                break;
        }
    }
}