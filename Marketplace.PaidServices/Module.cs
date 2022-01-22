using EventStore.ClientAPI;
using Marketplace.EventSourcing;
using Marketplace.EventStore;
using Marketplace.PaidServices.ClassifiedAds;
using Marketplace.PaidServices.Orders;
using Marketplace.PaidServices.Projections;
using Marketplace.RavenDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Marketplace.PaidServices;

public static class PaidServicesModule
{
    public static IMvcBuilder AddPaidServicesModule(
        this IMvcBuilder builder,
        string databaseName
    )
    {
        EventMappings.MapEventTypes();

        builder.Services.AddSingleton(
            c => new OrdersCommandService(c.GetStore())
        );

        builder.Services.AddSingleton(
            c => new ClassifiedAdCommandService(c.GetStore())
        );

        builder.Services.AddSingleton(
            c =>
            {
                var store = c.GetRequiredService<IDocumentStore>();
                store.CheckAndCreateDatabase(databaseName);
                const string subscriptionName = "servicesReadModels";

                IAsyncDocumentSession GetSession()
                {
                    return c.GetRequiredService<IDocumentStore>()
                        .OpenAsyncSession(databaseName);
                }

                return new SubscriptionManager(
                    c.GetRequiredService<IEventStoreConnection>(),
                    new RavenDbCheckpointStore(
                        GetSession, subscriptionName
                    ),
                    subscriptionName,
                    c.GetRequiredService<ILogger<SubscriptionManager>>(),
                    new RavenDbProjection<ReadModels.OrderDraft>(
                        c.GetRequiredService<ILogger<RavenDbProjection<ReadModels.OrderDraft>>>(),
                        GetSession,
                        DraftOrderProjection.GetHandler
                    ),
                    new RavenDbProjection<ReadModels.CompletedOrder>(
                        c.GetRequiredService<ILogger<RavenDbProjection<ReadModels.CompletedOrder>>>(),
                        GetSession,
                        CompletedOrderProjection.GetHandler
                    )
                );
            }
        );

        builder.Services.AddSingleton(
            c =>
            {
                var connection =
                    c.GetRequiredService<IEventStoreConnection>();
                const string subscriptionName = "servicesReactors";

                return new SubscriptionManager(
                    connection,
                    new EsCheckpointStore(connection, subscriptionName),
                    subscriptionName,
                    c.GetRequiredService<ILogger<SubscriptionManager>>(),
                    new OrderReactor(
                        c.GetRequiredService<ClassifiedAdCommandService>(),
                        c.GetRequiredService<ILogger<OrderReactor>>()
                    )
                );
            }
        );

        builder.AddApplicationPart(typeof(PaidServicesModule).Assembly);

        return builder;
    }

    private static IFunctionalAggregateStore GetStore(
        this IServiceProvider provider
    )
    {
        return new FunctionalStore(
            provider.GetRequiredService<IEventStoreConnection>()
        );
    }
}