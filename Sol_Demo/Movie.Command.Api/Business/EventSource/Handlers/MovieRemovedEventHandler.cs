using Framework.EventSource.Repository;
using MediatR;
using Movie.Command.Api.Business.EventSource.Abstract;
using Movie.Command.Api.Business.EventSource.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Handlers
{
    public sealed class MovieRemovedEventHandler : MovieBaseEventHandlerAbstract, INotificationHandler<MovieRemovedEvent>
    {
        private readonly IEventSourceRepository eventSourceRepository = null;

        public MovieRemovedEventHandler(IEventSourceRepository eventSourceRepository)
        {
            this.eventSourceRepository = eventSourceRepository;
        }

        async Task INotificationHandler<MovieRemovedEvent>.Handle(MovieRemovedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await base.PublishEventStoreAsync<MovieRemovedEvent>("MovieRemovedEvent", this.eventSourceRepository, notification);
            }
            catch
            {
                throw;
            }
        }
    }
}