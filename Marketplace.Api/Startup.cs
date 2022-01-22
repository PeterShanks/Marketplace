using EventStore.ClientAPI;
using Marketplace.Ads;
using Marketplace.Ads.Domain.Shared;
using Marketplace.EventStore;
using Marketplace.Infrastructure.Currency;
using Marketplace.Infrastructure.Profanity;
using Marketplace.Modules.Images;
using Marketplace.PaidServices;
using Marketplace.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Marketplace.Infrastructure.RavenDb.Configuration;


namespace Marketplace;

public class Startup
{
    public const string CookieScheme = "MarketplaceScheme";

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_ => EventStoreConnection.Create(
            Configuration.GetValue<string>("eventStore:connectionString"),
            ConnectionSettings.Create().DisableTls().KeepReconnecting(),
            Environment.ApplicationName
        ));

        var documentStore = ConfigureRavenDb(
            Configuration["ravenDb:server"]
        );

        services.AddSingleton(new ImageQueryService(ImageStorage.GetFile));
        services.AddSingleton(documentStore);

        services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();

        services.AddHostedService<EventStoreService>();

        services.AddControllers()
            .AddApplicationPart(GetType().Assembly)
            .AddAdsModule(
                "ClassifiedAds",
                ImageStorage.UploadFile
            )
            .AddUsersModule(
                "Users",
                new PurgomalumClient().CheckForProfanity)
            .AddPaidServicesModule("PaidServices");

        services.AddHttpClient();
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}