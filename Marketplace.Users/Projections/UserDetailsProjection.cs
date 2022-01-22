using Marketplace.RavenDb;
using Marketplace.Users.Messages;
using Raven.Client.Documents.Session;

namespace Marketplace.Users.Projections;

public static class UserDetailsProjection
{
    public static Func<Task> GetHandler(IAsyncDocumentSession session, object @event)
    {
        var getDbId = ReadModels.UserDetails.GetDatabaseId;

        return @event switch
        {
            Events.V1.UserRegistered e =>
                () => Create(e.UserId, e.DisplayName, e.FullName, session, getDbId),
            Events.V1.UserDisplayNameUpdated e =>
                () => Update(e.UserId, x => x.DisplayName = e.DisplayName, session, getDbId),
            Events.V1.ProfilePhotoUploaded e =>
                () => Update(e.UserId, x => x.PhotoUrl = e.PhotoUrl, session, getDbId),
            _ => () => Task.CompletedTask
        };
    }

    private static Task Create(
        Guid userId,
        string displayName,
        string fullName,
        IAsyncDocumentSession session,
        Func<Guid, string>? getDbId)
    {
        return session.StoreAsync(
            new ReadModels.UserDetails
            {
                Id = getDbId(userId),
                DisplayName = displayName,
                FullName = fullName
            });
    }

    private static Task Update(
        Guid id,
        Action<ReadModels.UserDetails> update,
        IAsyncDocumentSession session,
        Func<Guid, string>? getDbId)
    {
        return session.Update(getDbId(id), update);
    }
}