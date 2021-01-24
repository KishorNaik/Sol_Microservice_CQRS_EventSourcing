using AutoMapper;
using MediatR;
using Movie.Command.Api.Business.EventSource.Events;
using Movie.Command.Api.Business.Features.Commands;
using Movie.Command.Api.Cores.Repository;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.Features.Handlers
{
    public sealed class MovieRemoveCommandHandler : IRequestHandler<MovieRemoveCommand, bool>
    {
        private readonly IMovieRemoveRepository movieRemoveRepository = null;
        private readonly IMapper mapper = null;
        private readonly IMediator mediator = null;

        public MovieRemoveCommandHandler(IMovieRemoveRepository movieRemoveRepository, IMapper mapper, IMediator mediator)
        {
            this.movieRemoveRepository = movieRemoveRepository;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        private void PublishMovieRemovedEvent(Guid? aggregateId, MovieRemoveCommand movieRemoveCommand)
        {
            try
            {
                mediator.Publish<MovieRemovedEvent>(new MovieRemovedEvent()
                {
                    AggregateId = aggregateId,
                    MovieIdentity = movieRemoveCommand?.MovieIdentity
                });
            }
            catch
            {
                throw;
            }
        }

        async Task<bool> IRequestHandler<MovieRemoveCommand, bool>.Handle(MovieRemoveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregateId = await movieRemoveRepository?.MovieRemoveAsync(mapper.Map<MovieModel>(request));

                if (aggregateId != null)
                {
                    _ = Task.Factory.StartNew(() =>
                    {
                        PublishMovieRemovedEvent(aggregateId, request);
                    }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                }

                return (aggregateId != null) ? (dynamic)true : false;
            }
            catch
            {
                throw;
            }
        }
    }
}