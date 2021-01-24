using Dapper;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Framework.SqlClient.Helper;
using Framework.Models;

namespace Movie.Command.Api.Infrastructures.Abstract
{
    public abstract class MovieRepositoryWriteAbstract
    {
        protected Task<DynamicParameters> SetParameterAsync(String command, MovieModel movieModel)
        {
            return Task.Run(() =>
            {
                var dynamicParameter = new DynamicParameters();

                dynamicParameter.Add("@Command", command, DbType.String, ParameterDirection.Input);
                dynamicParameter.Add("@MovieIdentity", movieModel.MovieIdentity, DbType.Guid, ParameterDirection.Input);
                dynamicParameter.Add("@Title", movieModel.Title, direction: ParameterDirection.Input);
                dynamicParameter.Add("@ReleaseDate", movieModel.ReleaseDate, DbType.DateTime, ParameterDirection.Input);
                dynamicParameter.Add("@IsDelete", movieModel.IsDelete, DbType.Boolean, ParameterDirection.Input);

                return dynamicParameter;
            });
        }

        protected async Task<Guid?> ExecuteAsync(ISqlClientDbProvider sqlClientDbProvider, Task<DynamicParameters> taskDynamicParameters)
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
                                    ?.QueryAsync<MessageModel>(sql: "uspSetMovie", param: dynamicParameter, commandType: CommandType.StoredProcedure))
                                    ?.FirstOrDefault()
                                    ?.Message;

                            Guid aggregateId;
                            bool? isGuid = Guid.TryParse(data, out aggregateId);

                            return (isGuid == true ? (dynamic)aggregateId : (dynamic)null);
                        })
                        .ResultAsync<Guid?>();
            }
            catch
            {
                throw;
            }
        }
    }
}