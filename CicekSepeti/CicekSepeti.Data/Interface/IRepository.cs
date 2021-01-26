using CicekSepeti.Entities.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CicekSepeti.Data.Interface
{

    public interface IRepository<TModel> where TModel : EntityBase, new()
    {       
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<IEnumerable<TModel>> GetByParamAsync(Expression<Func<TModel, bool>> predicate=null);
        Task<TModel> GetByIdAsync(string id);
        Task AddAsync(TModel entity);
        Task<bool> AddRangeAsync(IEnumerable<TModel> entities);
        Task<TModel> UpdateAsync(string id, TModel entity);
        Task<TModel> UpdateAsync(TModel entity, Expression<Func<TModel, bool>> predicate);
        Task<TModel> DeleteAsync(TModel entity);
        Task<TModel> DeleteAsync(string id);
        Task<TModel> DeleteAsync(Expression<Func<TModel , bool>> predicate);
    }
}
