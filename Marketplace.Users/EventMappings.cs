using static Marketplace.EventSourcing.TypeMapper;
using static Marketplace.Users.Messages.Events;

namespace Marketplace.Users;

public static class EventMappings
{
    public static void MapEventTypes()
    {
        Map<V1.UserRegistered>(nameof(V1.UserRegistered));
        Map<V1.UserFullNameUpdated>(nameof(V1.UserFullNameUpdated));
        Map<V1.UserDisplayNameUpdated>(nameof(V1.UserDisplayNameUpdated));
        Map<V1.ProfilePhotoUploaded>(nameof(V1.ProfilePhotoUploaded));
    }
}