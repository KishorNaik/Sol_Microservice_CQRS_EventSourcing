using AutoMapper;
using MediatR;
using Movie.Command.Api.Business.EventSource.Events;
using Movie.Command.Api.Business.Features.Abstract;
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
    public sealed class MovieEditCommandHandler : MovieBaseCommandHandlerAbstract, IRequestHandler<MovieEditCommand, Object>
    {
        private readonly IMovieEditRepository movieEditRepository = null;
        private readonly IMapper mapper = null;
        private readonly IMediator mediator = null;

        public MovieEditCommandHandler(IMovieEditRepository movieEditRepository, IMapper mapper, IMediator mediator)
        {
            this.movieEditRepository = movieEditRepository;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        private void PublishMovieTitleChanged(Guid? aggregateId, MovieEditCommand movieEditCommand)
        {
            try
            {
                if (aggregateId != null)
                {
                    mediator.Publish<MovieTitleChangedEvent>(new MovieTitleChangedEvent()
                    {
                        AggregateId = aggregateId,
                        Title = movieEditCommand.Title
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        private void PublishMovieReleaseDateChanged(Guid? aggregateId, MovieEditCommand movieEditCommand)
        {
            try
            {
                if (aggregateId != null)
                {
                    mediator.Publish<MovieReleasedDateChangedEvent>(new MovieReleasedDateChangedEvent()
                    {
                        AggregateId = aggregateId,
                        Title = movieEditCommand.Title,
                        ReleaseDate = movieEditCommand.ReleaseDate
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        async Task<object> IRequestHandler<MovieEditCommand, object>.Handle(MovieEditCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var oldMovieModel = await movieEditRepository.MovieEditAsync(mapper.Map<MovieModel>(request));

                PublishEvents();

                return (oldMovieModel != null) ? (dynamic)true : (dynamic)await base.MovieExistsMessageAsync();

                // Publish All Events
                void PublishEvents()
                {
                    if (oldMovieModel != null)
                    {
                        // Note : Every Events run on different background thread otherwise it would give error
                        _ = Task.Factory.StartNew(() =>
                        {
                            if (oldMovieModel?.Title != request?.Title)
                            {
                                this.PublishMovieTitleChanged(oldMovieModel.AggregateId, request);
                            }
                        }, TaskCreationOptions.LongRunning).ConfigureAwait(false);

                        _ = Task.Factory.StartNew(() =>
                        {
                            if (oldMovieModel?.ReleaseDate != request?.ReleaseDate)
                            {
                                this.PublishMovieReleaseDateChanged(oldMovieModel.AggregateId, request);
                            }
                        }, TaskCreationOptions.LongRunning).ConfigureAwait(false);

                        //List<Action> actions = new List<Action>();
                        //actions.Add(() => this.PublishMovieTitleChanged(oldMovieModel.AggregateId, request));
                        //actions.Add(() => this.PublishMovieReleaseDateChanged(oldMovieModel.AggregateId, request));

                        //Parallel.Invoke(actions.ToArray());
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}