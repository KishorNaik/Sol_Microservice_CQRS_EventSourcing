using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Cores.Repository
{
    public interface IMovieEditRepository
    {
        Task<MovieModel> MovieEditAsync(MovieModel movieModel);
    }
}