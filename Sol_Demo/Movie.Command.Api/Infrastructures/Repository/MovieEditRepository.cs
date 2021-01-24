using Dapper;
using Framework.SqlClient.Helper;
using Movie.Command.Api.Cores.Repository;
using Movie.Command.Api.Infrastructures.Abstract;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Command.Api.Infrastructures.Repository
{
    public sealed class MovieEditRepository : MovieRepositoryWriteAbstract, IMovieEditRepository
    {
        private readonly ISqlClientDbProvider sqlClientDbProvider = null;

        public MovieEditRepository(ISqlClientDbProvider sqlClientDbProvider)
        {
            this.sqlClientDbProvider = sqlClientDbProvider;
        }

        private new async Task<MovieModel> ExecuteAsync(ISqlClientDbProvider sqlClientDbProvider, Task<DynamicParameters> taskDynamicParameters)
        {
            try
            {
                return
                   await
                   sqlClientDbProvider
                       .DapperBuilder
                       .OpenConnection(sqlClientDbProvider.GetConnection())
                       .Parameter(async () => await taskDynamicParameters)
                       .Command(async (dbConnection, dynamicParameter) =>
                       {
                           var data =
                                   (await
                                   dbConnection
                                   ?.QueryAsync<MovieModel>(sql: "uspSetMovie", param: dynamicParameter, commandType: CommandType.StoredProcedure))
                                   ?.FirstOrDefault();

                           return ((data?.MovieIdentity != null) ? (dynamic)data : (dynamic)null);
                       })
                       .ResultAsync<MovieModel>();
            }
            catch
            {
                throw;
            }
        }

        async Task<MovieModel> IMovieEditRepository.MovieEditAsync(MovieModel movieModel)
        {
            try
            {
                var dynamicParameterTask = base.SetParameterAsync("Update", movieModel);

                var data = await this.ExecuteAsync(sqlClientDbProvider, dynamicParameterTask);

                return data;
            }
            catch
            {
                throw;
            }
        }
    }
}