using Framework.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.EventSource.Events
{
    public sealed class MovieReleasedDateChangedEvent : IAggregateModel, INotification
    {
        public String Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public Guid? AggregateId { get; set; }
    }
}