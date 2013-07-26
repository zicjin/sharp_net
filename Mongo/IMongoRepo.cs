using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Mongo {
    public interface IMongoRepo<T> where T : IMEntity {
        MongoCollection<T> Collection { get; }

        void Delete(Expression<Func<T, bool>> criteria);
    }
}
