using System;
using System.Threading.Tasks;
using Marketplace.RavenDb;
using Raven.Client.Documents.Session;
using static Marketplace.Ads.Messages.Events;
using static Marketplace.Ads.Projections.ReadModels;

namespace Marketplace.Ads.Projections;

public static class ClassifiedAdDetailsProjection
{
    public static Func<Task> GetHandler(
        IAsyncDocumentSession session,
        object @event
    )
    {
        var getDbId = ClassifiedAdDetails.GetDatabaseId;

        return @event switch
        {
            V1.ClassifiedAdCreated e =>
                () => Create(e.Id, e.OwnerId, session, getDbId),
            V1.ClassifiedAdTitleChanged e =>
                () => Update(e.Id, ad => ad.Title = e.Title, session, getDbId),
            V1.ClassifiedAdTextUpdated e =>
                () => Update(e.Id, ad => ad.Description = e.AdText, session, getDbId),
            V1.ClassifiedAdPriceUpdated e =>
                () => Update(e.Id, ad =>
                {
                    ad.Price = e.Price;
                    ad.CurrencyCode = e.CurrencyCode;
                }, session, getDbId),
            V1.PictureAddedToAClassifiedAd e =>
                () => Update(e.ClassifiedAdId, ad => ad.PhotoUrls.Add(e.Url), session, getDbId),
            V1.ClassifiedAdDeleted e =>
                () => Delete(e.Id, session, getDbId),
            _ => () => Task.CompletedTask
        };
    }


    public static Task Create(
        Guid id,
        Guid ownerId,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId)
    {
        return session.Create<ClassifiedAdDetails>(
            x =>
            {
                x.Id = getDbId(id);
                x.SellerId = ownerId;
            }
        );
    }

    public static Task Update(
        Guid id,
        Action<ClassifiedAdDetails> update,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId)
    {
        return session.Update(getDbId(id), update);
    }

    public static Task Delete(
        Guid id,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId)
    {
        session.Delete(getDbId(id));
        return Task.CompletedTask;
    }
}