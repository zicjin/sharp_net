namespace sharp_net.Mongo {
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Abstract Entity for all the BusinessEntities.
    /// </summary>
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MEntity : IMEntity {
        /// <summary>
        /// Gets or sets the id for this object (the primary record for an entity).
        /// </summary>
        /// <value>The id for this object (the primary record for an entity).</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
