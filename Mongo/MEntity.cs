namespace sharp_net.Mongo {
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Runtime.Serialization;

    //Mongodb Respository's Entity有DataContract特性，所以无法被json.net序列化，只能使用DataContractJsonSerializer。DataContractJsonSerializer 不能忽略大小写
    //var data = JsonHelper.JsonDeserialize<IEnumerable<GgpttCard>>(datajson);
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MEntity : IMEntity {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
