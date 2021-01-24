using Framework.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Events
{
    public sealed class MovieTitleChangedEvent : IAggregateModel, INotification
    {
        public String Title { get; set; }

        public Guid? AggregateId { get; set; }
    }
}