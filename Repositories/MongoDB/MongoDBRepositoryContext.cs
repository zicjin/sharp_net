using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using sharp_net;
using sharp_net.Repositories;
using sharp_net.Repositories.MongoDB;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Reflection;

namespace sharp_net.Repositories.MongoDB {
    public class MongoDBRepositoryContext : Disposable, IMongoDBRepositoryContext {

        private readonly Guid id = Guid.NewGuid();
        private readonly MongoServer server;
        private readonly MongoDatabase database;
        private readonly IMongoDBSettings settings;
        private readonly object syncObj = new object();
        private readonly Dictionary<Type, MongoCollection> mongoCollections = new Dictionary<Type, MongoCollection>();
        private readonly ThreadLocal<List<object>> localNewCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localModifiedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localDeletedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<bool> localCommitted = new ThreadLocal<bool>(() => true);

        public MongoDBRepositoryContext(string settingkey) {
            this.settings = IocLocator.Instance.GetImple<IMongoDBSettings>(settingkey);
            server = new MongoServer(settings.ServerSettings);
            database = server.GetDatabase(settings.GetDatabaseSettings(server));
        }

        private void ClearRegistrations() {
            this.localNewCollection.Value.Clear();
            this.localModifiedCollection.Value.Clear();
            this.localDeletedCollection.Value.Clear();
        }

        protected override void DisposeCustom() {
            server.Disconnect();
            this.localCommitted.Dispose();
            this.localDeletedCollection.Dispose();
            this.localModifiedCollection.Dispose();
            this.localNewCollection.Dispose();
        }

        public static void RegisterConventions(bool autoGenerateID = true, bool localDateTime = true) {
            RegisterConventions(autoGenerateID, localDateTime, null);
        }

        public static void RegisterConventions(bool autoGenerateID, bool localDateTime, ConventionProfile additionConventions) {
            var convention = new ConventionProfile();
            convention.SetIdMemberConvention(new NamedIdMemberConvention("id", "Id", "ID"));

            if (autoGenerateID)
                convention.SetIdGeneratorConvention(new GuidIDGeneratorConvention());

            if (localDateTime)
                convention.SetSerializationOptionsConvention(new UseLocalDateTimeConvention());

            if (additionConventions != null)
                convention.Merge(additionConventions);

            BsonClassMap.RegisterConventions(convention, type => true);
        }

        public IMongoDBSettings Settings {
            get { return settings; }
        }

        public MongoCollection GetCollectionForType(Type type) {
            lock (syncObj) {
                if (this.mongoCollections.ContainsKey(type))
                    return this.mongoCollections[type];
                else {
                    MongoCollection mongoCollection = null;
                    if (settings.MapTypeToCollectionName != null)
                        mongoCollection = this.database.GetCollection(settings.MapTypeToCollectionName(type));
                    else
                        mongoCollection = this.database.GetCollection(type.Name);
                    this.mongoCollections.Add(type, mongoCollection);
                    return mongoCollection;
                }
            }
        }

        public Guid ID {
            get { return id; }
        }

        public void RegisterNew<T>(T obj) where T : class, IAggregateRoot {
            if (localModifiedCollection.Value.Contains(obj))
                throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            if (localNewCollection.Value.Contains(obj))
                throw new InvalidOperationException("The object has already been registered as a new object.");
            localNewCollection.Value.Add(obj);
            localCommitted.Value = false;
        }

        public void RegisterModified<T>(T obj) where T : class, IAggregateRoot {
            if (localDeletedCollection.Value.Contains(obj))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!localModifiedCollection.Value.Contains(obj) && !localNewCollection.Value.Contains(obj))
                localModifiedCollection.Value.Add(obj);
            localCommitted.Value = false;
        }

        public void RegisterDeleted<T>(T obj) where T : class, IAggregateRoot {
            if (localNewCollection.Value.Contains(obj)) {
                if (localNewCollection.Value.Remove(obj))
                    return;
            }
            bool removedFromModified = localModifiedCollection.Value.Remove(obj);
            bool addedToDeleted = false;
            if (!localDeletedCollection.Value.Contains(obj)) {
                localDeletedCollection.Value.Add(obj);
                addedToDeleted = true;
            }
            localCommitted.Value = !(removedFromModified || addedToDeleted);
        }

        public IRepository<T> GetRepository<T>() where T : class, IAggregateRoot {
            return IocLocator.Instance.GetImple<IRepository<T>>(eDbType.Mongo.ToString(), new { context = this });
        }

        public bool DTCompatible {
            get { return false; }
        }

        public bool Committed {
            get { return localCommitted.Value; }
        }

        public void Commit() {
            lock (syncObj) {
                foreach (var newObj in this.localNewCollection.Value) {
                    MongoCollection collection = this.GetCollectionForType(newObj.GetType());
                    collection.Insert(newObj);
                }
                foreach (var modifiedObj in this.localModifiedCollection.Value) {
                    MongoCollection collection = this.GetCollectionForType(modifiedObj.GetType());
                    collection.Save(modifiedObj);
                }
                foreach (var delObj in this.localDeletedCollection.Value) {
                    Type objType = delObj.GetType();
                    PropertyInfo propertyInfo = objType.GetProperty("ID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (propertyInfo == null)
                        throw new InvalidOperationException("Cannot delete an object which doesn't contain an ID property.");
                    Guid id = (Guid)propertyInfo.GetValue(delObj, null);
                    MongoCollection collection = this.GetCollectionForType(objType);
                    IMongoQuery query = Query.EQ("_id", id);
                    collection.Remove(query);
                }
                this.ClearRegistrations();
                this.localCommitted.Value = true;
            }
        }

        public void Rollback() {
            this.localCommitted.Value = false;
        }

    }
}
