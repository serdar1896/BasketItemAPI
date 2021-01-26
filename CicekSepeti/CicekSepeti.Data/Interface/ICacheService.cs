using CicekSepeti.Data.CacheService.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CicekSepeti.Data.Interface
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<IEnumerable<T>> GetByPatternAsync<T>(string pattern);
        Task AddAsync(string key, object data);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task Clear();
        Task<bool> AnyAsync(string key);
    }
}
