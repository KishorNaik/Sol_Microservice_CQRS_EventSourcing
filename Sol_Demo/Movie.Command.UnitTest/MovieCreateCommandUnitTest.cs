using AutoMapper;

using Framework.Models;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Movie.Command.Api.Business.EventSource.Events;
using Movie.Command.Api.Business.Features.Commands;
using Movie.Command.Api.Business.Features.Handlers;
using Movie.Command.Api.Cores.Repository;
using Movies.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Movie.Command.UnitTest
{
    [TestClass]
    public class MovieCreateCommandUnitTest
    {
        private readonly Mock<IMovieCreateRepository> movieCreateRepositoryMock = null;
        private readonly Mock<IMapper> mapperMock = null;
        private readonly Mock<IMediator> mediatorMock = null;

        private IRequestHandler<MovieCreateCommand, Object> movieCreateCommandHandler = null;

        public MovieCreateCommandUnitTest()
        {
            movieCreateRepositoryMock = new Mock<IMovieCreateRepository>();
            mapperMock = new Mock<IMapper>();
            mediatorMock = new();

            movieCreateCommandHandler = new MovieCreateCommandHandler(movieCreateRepositoryMock.Object, mapperMock.Object, mediatorMock.Object);
        }

        private MovieCreateCommand MovieCreateCommandData()
        {
            var movieCreateCommand = new MovieCreateCommand()
            {
                Title = "X-Men",
                ReleaseDate = new DateTime(2020, 10, 1)
            };

            return movieCreateCommand;
        }

        [TestMethod]
        public async Task AddMovie_Success()
        {
            var movieCreateCommand = this.MovieCreateCommandData();

            movieCreateRepositoryMock
                .Setup((r) => r.MoviewCreateAsync(It.IsAny<MovieModel>()))
                .ReturnsAsync(Guid.NewGuid());

            mediatorMock
                .Setup((r) => r.Publish<MovieCreatedEvent>(It.IsAny<MovieCreatedEvent>(), new System.Threading.CancellationToken()));

            var response = await movieCreateCommandHandler.Handle(movieCreateCommand, new System.Threading.CancellationToken());

            Assert.IsTrue(Convert.ToBoolean(response));
        }

        [TestMethod]
        public async Task AddMovie_MovieExists()
        {
            var movieCreateCommand = this.MovieCreateCommandData();

            movieCreateRepositoryMock
                .Setup((r) => r.MoviewCreateAsync(It.IsAny<MovieModel>()))
                .ReturnsAsync((Guid?)null); // If it is null then movie title already exists in database

            var response = await movieCreateCommandHandler.Handle(movieCreateCommand, new System.Threading.CancellationToken());

            Assert.IsTrue(response is ErrorModel);
        }

        [TestMethod]
        public async Task AddMovie_ThrowException()
        {
            var movieCreateCommand = this.MovieCreateCommandData();

            movieCreateRepositoryMock
                .Setup((r) => r.MoviewCreateAsync(It.IsAny<MovieModel>()))
                .Throws<Exception>();

            try
            {
                var response = await movieCreateCommandHandler.Handle(movieCreateCommand, new System.Threading.CancellationToken());
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}