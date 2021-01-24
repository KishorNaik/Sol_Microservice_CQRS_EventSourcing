using Framework.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Events
{
    public sealed class MovieRemovedEvent : INotification, IAggregateModel
    {
        public Guid? AggregateId { get; set; }

        public Guid? MovieIdentity { get; set; }
    }
}