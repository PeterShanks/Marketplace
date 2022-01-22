using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.RavenDb;
using Raven.Client.Documents.Session;
using static Marketplace.Ads.Messages.Events;
using static Marketplace.Ads.Projections.ReadModels;

namespace Marketplace.Ads.Projections;

public static class MyClassifiedAdsProjection
{
    public static Func<Task> GetHandler(
        IAsyncDocumentSession session,
        object @event
    )
    {
        var getDbId = MyClassifiedAds.GetDatabaseId;

        return @event switch
        {
            V1.ClassifiedAdCreated e =>
                () => CreateOrUpdate(e.OwnerId,
                    myAds => myAds.MyAds.Add(
                        new MyClassifiedAds.MyAd {Id = e.Id}
                    ),
                    () => new MyClassifiedAds
                    {
                        Id = getDbId(e.OwnerId),
                        MyAds = new List<MyClassifiedAds.MyAd>()
                    },
                    session,
                    getDbId
                ),
            V1.ClassifiedAdTitleChanged e =>
                () => UpdateOneAd(e.OwnerId, e.Id,
                    myAd => myAd.Title = e.Title,
                    session,
                    getDbId
                ),
            V1.ClassifiedAdTextUpdated e =>
                () => UpdateOneAd(e.OwnerId,
                    e.Id,
                    myAd => myAd.Description = e.AdText,
                    session,
                    getDbId
                ),
            V1.ClassifiedAdPriceUpdated e =>
                () => UpdateOneAd(e.OwnerId,
                    e.Id,
                    myAd => myAd.Price = e.Price,
                    session,
                    getDbId
                ),
            V1.PictureAddedToAClassifiedAd e =>
                () => UpdateOneAd(e.OwnerId,
                    e.ClassifiedAdId,
                    myAd => myAd.PhotoUrls.Add(e.Url),
                    session,
                    getDbId
                ),
            V1.ClassifiedAdDeleted e =>
                () => Update(e.OwnerId,
                    myAds => myAds.MyAds.RemoveAll(a => a.Id == e.Id),
                    session,
                    getDbId
                ),
            _ => () => Task.CompletedTask
        };
    }

    public static Task CreateOrUpdate(
        Guid id,
        Action<MyClassifiedAds> update,
        Func<MyClassifiedAds> create,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId)
    {
        return session.UpsertItem(getDbId(id), update, create);
    }

    public static Task Update(
        Guid id,
        Action<MyClassifiedAds> update,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId
    )
    {
        return session.Update(getDbId(id), update);
    }

    public static Task UpdateOneAd(
        Guid id,
        Guid adId,
        Action<MyClassifiedAds.MyAd> update,
        IAsyncDocumentSession session,
        Func<Guid, string> getDbId
    )
    {
        return Update(id, myAds =>
            {
                var ad = myAds.MyAds
                    .FirstOrDefault(x => x.Id == adId);
                if (ad != null) update(ad);
            },
            session,
            getDbId);
    }
}