using Framework.MongoDbClient.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MongoDbClient.Configurations
{
    public static class MongoDbProviderExtension
    {
        public static void AddMongoDbProvider(this IServiceCollection services, string connectionString, string database)
        {
            services.AddTransient<IMongoDbClientDbProvider, MongoDbClientDbProvider>((config) =>
            {
                return new MongoDbClientDbProvider(connectionString, database);
            });
        }
    }
}