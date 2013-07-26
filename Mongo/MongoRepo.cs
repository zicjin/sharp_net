﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using sharp_net.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace sharp_net.Mongo {
    public class MongoRepo<T> where T : IMEntity {
        private MongoCollection<T> collection;
        public MongoCollection<T> Collection {
            get {
                return this.collection;
            }
        }

        public MongoRepo()
            : this(MongoUtil.GetDefaultConnectionString()) {
        }

        public MongoRepo(string connectionString) {
            this.collection = MongoUtil.GetCollectionFromConnectionString<T>(connectionString);
        }

        public void Delete(Expression<Func<T, bool>> criteria) {
            foreach (T entity in this.collection.AsQueryable<T>().Where(criteria)) {
                this.collection.Remove(Query.EQ("_id", new ObjectId(entity.Id)));
            }
        }

    }
}