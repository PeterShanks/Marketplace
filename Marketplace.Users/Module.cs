using EventStore.ClientAPI;
using Marketplace.EventStore;
using Marketplace.RavenDb;
using Marketplace.Users.Auth;
using Marketplace.Users.Domain.Shared;
using Marketplace.Users.Projections;
using Marketplace.Users.UserProfiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Marketplace.Users;

public static class Module
{
    private const string SubscriptionName = "usersSubscription";

    public static IMvcBuilder AddUsersModule(
        this IMvcBuilder builder,
        string databaseName,
        CheckTextForProfanity profanityCheck
    )
    {
        EventMappings.MapEventTypes();

        builder.Services.AddSingleton(
                provider =>
                    new UserProfileCommandService(
                        new EsAggregateStore(
                            provider.GetRequiredService<IEventStoreConnection>(),
                            provider.GetRequiredService<ILogger<EsAggregateStore>>()),
                        provider.GetRequiredService<ILogger<UserProfileCommandService>>(),
                        profanityCheck
                    )
            )
            .AddSingleton<GetUsersModuleSession>(
                provider =>
                {
                    var store = provider.GetRequiredService<IDocumentStore>();
                    store.CheckAndCreateDatabase(databaseName);

                    IAsyncDocumentSession GetSession()
                    {
                        return store.OpenAsyncSession(databaseName);
                    }

                    return GetSession;
                })
            .AddSingleton(
                provider =>
                {
                    var getSession = provider.GetRequiredService<GetUsersModuleSession>();

                    return new SubscriptionManager(
                        provider.GetRequiredService<IEventStoreConnection>(),
                        new RavenDbCheckpointStore(
                            () => getSession(),
                            SubscriptionName
                        ),
                        SubscriptionName,
                        provider.GetRequiredService<ILogger<SubscriptionManager>>(),
                        new RavenDbProjection<ReadModels.UserDetails>(
                            provider.GetRequiredService<ILogger<RavenDbProjection<ReadModels.UserDetails>>>(),
                            () => getSession(),
                            UserDetailsProjection.GetHandler
                        )
                    );
                }
            )
            .AddSingleton<AuthService>();


        builder.AddApplicationPart(typeof(Module).Assembly);

        return builder;
    }
}