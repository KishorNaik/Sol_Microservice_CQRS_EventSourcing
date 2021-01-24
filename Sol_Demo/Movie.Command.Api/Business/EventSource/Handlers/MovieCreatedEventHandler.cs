using Framework.EventSource.Models;
using Framework.EventSource.Repository;
using MediatR;
using Movie.Command.Api.Business.EventSource.Abstract;
using Movie.Command.Api.Business.EventSource.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Handlers
{
    public sealed class MovieCreatedEventHandler : MovieBaseEventHandlerAbstract, INotificationHandler<MovieCreatedEvent>
    {
        private readonly IEventSourceRepository eventSourceRepository = null;

        public MovieCreatedEventHandler(IEventSourceRepository eventSourceRepository)
        {
            this.eventSourceRepository = eventSourceRepository;
        }

        async Task INotificationHandler<MovieCreatedEvent>.Handle(MovieCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await base.PublishEventStoreAsync<MovieCreatedEvent>("MovieCreatedEvent", this.eventSourceRepository, notification);
            }
            catch
            {
                throw;
            }
        }
    }
}