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
    public sealed class MovieTitleChangedEventHandler : MovieBaseEventHandlerAbstract, INotificationHandler<MovieTitleChangedEvent>
    {
        private readonly IEventSourceRepository eventSourceRepository = null;

        public MovieTitleChangedEventHandler(IEventSourceRepository eventSourceRepository)
        {
            this.eventSourceRepository = eventSourceRepository;
        }

        async Task INotificationHandler<MovieTitleChangedEvent>.Handle(MovieTitleChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await base.PublishEventStoreAsync<MovieTitleChangedEvent>("MovieTitleChangedEvent", this.eventSourceRepository, notification);
            }
            catch
            {
                throw;
            }
        }
    }
}