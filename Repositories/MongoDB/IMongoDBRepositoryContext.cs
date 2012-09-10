using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace sharp_net.Repositories.MongoDB {
    public interface IMongoDBRepositoryContext : IRepositoryContext {
        /// <summary>
        /// Gets a <see cref="IMongoDBSettings"/> instance which contains the settings
        /// information used by current context.
        /// </summary>
        IMongoDBSettings Settings { get; }
        /// <summary>
        /// Gets the <see cref="MongoCollection"/> instance by the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> object.</param>
        /// <returns>The <see cref="MongoCollection"/> instance.</returns>
        MongoCollection GetCollectionForType(Type type);
    }
}
