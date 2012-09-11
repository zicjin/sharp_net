using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace sharp_net.Repositories.MongoDB {
    public class GuidIDGeneratorConvention : IIdGeneratorConvention {

        [Obsolete] //因为ObjectId性能比Guid更好，所以倾向于不使用Guid作为主键
        public virtual IIdGenerator GetIdGenerator(MemberInfo memberInfo) {
            if (memberInfo.Name == "ID" || memberInfo.Name == "Id" || memberInfo.Name == "id" || memberInfo.Name == "_id") {
                switch (memberInfo.MemberType) {
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        if (propertyInfo.PropertyType == typeof(Guid) || propertyInfo.PropertyType == typeof(Guid?))
                            return new GuidGenerator();
                        break;
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = (FieldInfo)memberInfo;
                        if (fieldInfo.FieldType == typeof(Guid) || fieldInfo.FieldType == typeof(Guid?))
                            return new GuidGenerator();
                        break;
                    default:
                        break;
                }
            }
            return null;
        }

    }
}
