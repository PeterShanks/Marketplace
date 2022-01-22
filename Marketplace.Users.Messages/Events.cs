namespace Marketplace.Users.Messages;

public static class Events
{
    public static class V1
    {
        public record UserRegistered(Guid UserId, string FullName, string DisplayName);

        public record ProfilePhotoUploaded(Guid UserId, string PhotoUrl);

        public record UserFullNameUpdated(Guid UserId, string FullName);

        public record UserDisplayNameUpdated(Guid UserId, string DisplayName);
    }
}