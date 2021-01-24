using Framework.EventSource.Repository;
using Framework.MongoDbClient.Configurations;
using Framework.MongoDbClient.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventSource.Configurations
{
    public static class EventSourceRepositoryServiceExtension
    {
        public static void AddEventSource(this IServiceCollection services, String mongoDbConnectionString, String mongoDatabaseName)
        {
            services.AddMongoDbProvider(mongoDbConnectionString, mongoDatabaseName);
            services.AddTransient<IEventSourceRepository, EventSourceRepository>();

            // In Case Threading Scope Issue.
            //services.AddTransient<IEventSourceRepository, EventSourceRepository>((config) =>
            //{
            //    var mongoDbClientDbProvider = config.GetRequiredService<IMongoDbClientDbProvider>();
            //    return new EventSourceRepository(mongoDbClientDbProvider);
            //});
        }
    }
}