using Framework.EventSource.Models;
using Framework.MongoDbClient.Helper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventSource.Repository
{
    public interface IEventSourceRepository
    {
        Task AddEventStoreAsync(EventSourceModel eventSourceModel);
    }

    public sealed class EventSourceRepository : IEventSourceRepository
    {
        private readonly IMongoDatabase mongoDatabase = null;

        public EventSourceRepository(IMongoDbClientDbProvider mongoDbClientDbProvider)
        {
            this.mongoDatabase = mongoDbClientDbProvider.GetConnectionWithDatabase;
        }

        async Task IEventSourceRepository.AddEventStoreAsync(EventSourceModel eventSourceModel)
        {
            try
            {
                await
                    mongoDatabase
                    ?.GetCollection<EventSourceModel>("EventSource")
                    ?.InsertOneAsync(eventSourceModel);
            }
            catch
            {
                throw;
            }
        }
    }
}