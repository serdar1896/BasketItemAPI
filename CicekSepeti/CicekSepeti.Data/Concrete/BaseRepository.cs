using CicekSepeti.Data.Interface;
using CicekSepeti.Entities.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CicekSepeti.Data.Concrete
{
    public class BaseRepository<TModel> : IRepository<TModel> where TModel : EntityBase, new()
    {
        private readonly IMongoCollection<TModel> mongoCollection;

        public BaseRepository(ICicekSepetiDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            mongoCollection = database.GetCollection<TModel>(typeof(TModel).Name.ToLowerInvariant());
        } 

        public virtual async Task<IEnumerable<TModel>> GetByParamAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            return predicate == null
                ? await mongoCollection.AsQueryable().ToListAsync()
                : mongoCollection.AsQueryable().Where(predicate).ToList();
        }
     
        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await mongoCollection.Find(x => true).ToListAsync();
        }      


        public virtual async Task<TModel> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(TModel entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
             await mongoCollection.InsertOneAsync(entity, options);
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TModel> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await mongoCollection.BulkWriteAsync((IEnumerable<WriteModel<TModel>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<TModel> UpdateAsync(string id, TModel entity)
        {
            return await mongoCollection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<TModel> UpdateAsync(TModel entity, Expression<Func<TModel, bool>> predicate)
        {
            return await mongoCollection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<TModel> DeleteAsync(TModel entity)
        {
            return await mongoCollection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<TModel> DeleteAsync(string id)
        {
            return await mongoCollection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public virtual async Task<TModel> DeleteAsync(Expression<Func<TModel, bool>> filter)
        {
            return await mongoCollection.FindOneAndDeleteAsync(filter);
        }

    }  
}
