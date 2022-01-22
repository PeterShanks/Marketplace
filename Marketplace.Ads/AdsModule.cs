using System;
using EventStore.ClientAPI;
using Marketplace.Ads.ClassifiedAds;
using Marketplace.Ads.Domain.Shared;
using Marketplace.Ads.Projections;
using Marketplace.EventStore;
using Marketplace.RavenDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Marketplace.Ads;

public static class AdsModule
{
    private const string SubscriptionName = "adsSubscription";

    public static IMvcBuilder AddAdsModule(
        this IMvcBuilder builder,
        string databaseName,
        UploadFile uploadFile
    )
    {
        EventMappings.MapEventTypes();

        builder.Services.AddSingleton(
            c => new ClassifiedAdsCommandService(
                new EsAggregateStore(
                    c.GetEsConnection(),
                    c.GetRequiredService<ILogger<EsAggregateStore>>()),
                c.GetRequiredService<ILogger<ClassifiedAdsCommandService>>(),
                c.GetRequiredService<ICurrencyLookup>(),
                uploadFile)
        );

        builder.Services.AddSingleton(
            c =>
            {
                var store = c.GetRavenStore();
                store.CheckAndCreateDatabase(databaseName);

                IAsyncDocumentSession GetSession()
                {
                    return c.GetRavenStore()
                        .OpenAsyncSession(databaseName);
                }

                return new SubscriptionManager(
                    c.GetEsConnection(),
                    new RavenDbCheckpointStore(
                        GetSession,
                        SubscriptionName
                    ),
                    SubscriptionName,
                    c.GetRequiredService<ILogger<SubscriptionManager>>(),
                    new RavenDbProjection<ReadModels.ClassifiedAdDetails>(
                        c.GetRequiredService<ILogger<RavenDbProjection<ReadModels.ClassifiedAdDetails>>>(),
                        GetSession,
                        ClassifiedAdDetailsProjection.GetHandler
                    ),
                    new RavenDbProjection<ReadModels.MyClassifiedAds>(
                        c.GetRequiredService<ILogger<RavenDbProjection<ReadModels.MyClassifiedAds>>>(),
                        GetSession,
                        MyClassifiedAdsProjection.GetHandler
                    )
                );
            });

        builder.AddApplicationPart(typeof(AdsModule).Assembly);

        return builder;
    }

    private static IDocumentStore GetRavenStore(
        this IServiceProvider provider
    )
    {
        return provider.GetRequiredService<IDocumentStore>();
    }

    private static IEventStoreConnection GetEsConnection(
        this IServiceProvider provider
    )
    {
        return provider.GetRequiredService<IEventStoreConnection>();
    }
}