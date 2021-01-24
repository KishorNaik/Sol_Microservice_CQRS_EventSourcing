using Framework.EventSource.Models;
using Framework.EventSource.Repository;
using Framework.Models;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Abstract
{
    public abstract class MovieBaseEventHandlerAbstract
    {
        protected async Task PublishEventStoreAsync<TEvent>(string eventName, IEventSourceRepository eventSourceRepository, TEvent @event)
            where TEvent : INotification, IAggregateModel
        {
            try
            {
                // convert event model into json
                var movieEventJson = JsonConvert.SerializeObject(@event);

                // Generate Event Source Model
                EventSourceModel eventSourceModel = new()
                {
                    AggregateId = Convert.ToString(@event.AggregateId),
                    EventName = eventName,
                    PayLoad = movieEventJson
                };

                // Push Event Source
                await
                       eventSourceRepository
                       ?.AddEventStoreAsync(eventSourceModel);
            }
            catch
            {
                throw;
            }
        }
    }
}