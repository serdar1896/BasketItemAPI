using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CicekSepeti.Data.CacheService.Redis
{
    public class RedisServer
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private string configurationString;
        public RedisServer(IConfiguration configuration)
        {           
            CreateRedisConfigurationString(configuration);
            _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationString);
            Database = _connectionMultiplexer.GetDatabase();
        }

        public IDatabase Database { get; }
        public IServer Server=>_connectionMultiplexer.GetServer(configurationString);
        public async Task FlushDatabaseAsync()
        {
           await _connectionMultiplexer.GetServer(configurationString).FlushDatabaseAsync();
        }
        private void CreateRedisConfigurationString(IConfiguration configuration)
        {
            string host = configuration.GetSection("RedisConfiguration:Host").Value;
            string port = configuration.GetSection("RedisConfiguration:Port").Value;
            string password= configuration.GetSection("RedisConfiguration:Password").Value;
            configurationString = $"{host}:{port},password={password}";
        }

    }
}