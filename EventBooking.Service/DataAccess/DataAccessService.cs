using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace EventBooking.Service
{
    public class DataAccessService : IDataAccessService
    {

        private readonly IConfiguration config;

        public int CurrentPersonId { get; set; }
        public string Role { get; set; }
        public string UserContext { get; set; }

        public DataAccessService(IConfiguration configuration)
        {
            config = configuration;
        }

        public async Task<IDbConnection> GetConnection()
        {
            var conn = new SqlConnection(config.GetConnectionString("DefaultConnection")); // append application name here. and handle user id
            conn.Open();
            await conn.ExecuteAsync("SpSessionContextTsk",
                new { UserContext },
                commandType: CommandType.StoredProcedure
                );
            return conn;
        }

        public async Task ActionProcedure(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                await conn.ExecuteAsync(spName, param,
                    //new { Json = param is string ? param : us.SerializeObject(param)},
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<T> ActionProcedure<T>(string spName, object param)
        {
            var dp = new DynamicParameters();
            foreach (var key in param.GetType().GetProperties())
            {
                dp.Add(key.Name, key.GetValue(param));
            }
            //dp.Add("@Json", param is string ? param : us.SerializeObject(param), direction: ParameterDirection.Input);
            dp.Add("@Result", "", direction: ParameterDirection.Output);
            using (var conn = await GetConnection())
            {
                await conn.ExecuteAsync(spName, dp, commandType: CommandType.StoredProcedure);
                return dp.Get<T>("@Result");
            }
        }

        public async Task ActionProcedure(string spName, DynamicParameters dp)
        {
            using (var conn = await GetConnection())
            {
                await conn.ExecuteAsync(spName, dp, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
        }

        public async Task<IEnumerable<T>> RetrievalProcedureWithJsonInput<T>(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                var result = await conn.QueryAsync<T>(spName,
                new { Json = UtilityService.SerializeObject(param) },
                commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<IEnumerable<T>> RetrievalProcedure<T>(string spName, DynamicParameters dp)
        {
            using (var conn = await GetConnection())
            {
                var result = await conn.QueryAsync<T>(spName, dp, commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<IEnumerable<T>> RetrievalProcedure<T>(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                var result = await conn.QueryAsync<T>(spName,
                    param,
                commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<T> RetrievalProcedureSingleWithJsonInput<T>(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                var result = await conn.ExecuteScalarAsync<T>(spName,
                new { Json = UtilityService.SerializeObject(param) },
                commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<T> RetrievalProcedureSingle<T>(string spName, DynamicParameters dp)
        {
            using (var conn = await GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(spName, dp, commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<T> RetrievalProcedureSingle<T>(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(spName,
                    param,
                commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }

        public async Task<T> RetrievalProcedureScalar<T>(string spName, object param)
        {
            //var asdf = bs.SerializeObject(param);
            using (var conn = await GetConnection())
            {
                var result = await conn.ExecuteScalarAsync<T>(spName,
                    param,
                commandType: CommandType.StoredProcedure);
                conn.Close();
                return result;
            }
        }
    }
}
