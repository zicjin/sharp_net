namespace sharp_net.Mongo {
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Runtime.Serialization;

    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MEntity : IMEntity {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
