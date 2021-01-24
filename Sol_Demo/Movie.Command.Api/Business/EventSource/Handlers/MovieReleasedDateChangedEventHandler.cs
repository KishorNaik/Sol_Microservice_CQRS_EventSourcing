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
    public sealed class MovieReleasedDateChangedEventHandler : MovieBaseEventHandlerAbstract, INotificationHandler<MovieReleasedDateChangedEvent>
    {
        private readonly IEventSourceRepository eventSourceRepository = null;

        public MovieReleasedDateChangedEventHandler(IEventSourceRepository eventSourceRepository)
        {
            this.eventSourceRepository = eventSourceRepository;
        }

        async Task INotificationHandler<MovieReleasedDateChangedEvent>.Handle(MovieReleasedDateChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await base.PublishEventStoreAsync<MovieReleasedDateChangedEvent>("MovieReleasedDateChangedEvent", this.eventSourceRepository, notification);
            }
            catch
            {
                throw;
            }
        }
    }
}