using AutoMapper;
using MediatR;
using Movie.Command.Api.Business.EventSource.Events;
using Movie.Command.Api.Business.Features.Abstract;
using Movie.Command.Api.Business.Features.Commands;
using Movie.Command.Api.Cores.Repository;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Movie.Command.Api.Business.Features.Handlers
{
    public sealed class MovieCreateCommandHandler : MovieBaseCommandHandlerAbstract, IRequestHandler<MovieCreateCommand, object>
    {
        private readonly IMovieCreateRepository movieCreateRepository = null;
        private readonly IMapper mapper = null;
        private readonly IMediator mediator = null;

        public MovieCreateCommandHandler(IMovieCreateRepository movieCreateRepository, IMapper mapper, IMediator mediator)
        {
            this.movieCreateRepository = movieCreateRepository;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        private void PublishMovieCreatedEvent(Guid? aggregateId, MovieCreateCommand movieCreateCommand)
        {
            try
            {
                mediator.Publish<MovieCreatedEvent>(new()
                {
                    AggregateId = aggregateId,
                    Title = movieCreateCommand.Title,
                    ReleaseDate = movieCreateCommand.ReleaseDate
                });
            }
            catch
            {
                throw;
            }
        }

        async Task<object> IRequestHandler<MovieCreateCommand, object>.Handle(MovieCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aggregateId = await movieCreateRepository.MoviewCreateAsync(mapper.Map<MovieModel>(request));

                if (aggregateId != null)
                {
                    _ = Task.Factory.StartNew(() =>
                    {
                        this.PublishMovieCreatedEvent(aggregateId, request);
                    }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                }

                return (aggregateId != null) ? (dynamic)true : (dynamic)await base.MovieExistsMessageAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}