using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace zic_dotnet.Repositories.MongoDB {
    public class GuidIDGeneratorConvention : IIdGeneratorConvention {
        #region IIdGeneratorConvention Members
        /// <summary>
        /// Gets the Id generator for an Id member.
        /// </summary>
        /// <param name="memberInfo">The member.</param>
        /// <returns>An Id generator.</returns>
        public virtual IIdGenerator GetIdGenerator(MemberInfo memberInfo) {
            if (memberInfo.DeclaringType.GetInterfaces().Any(intf => intf == typeof(DomainInt)) &&
                (memberInfo.Name == "ID" || memberInfo.Name == "Id" || memberInfo.Name == "iD" ||
                memberInfo.Name == "id" || memberInfo.Name == "_id")) {
                switch (memberInfo.MemberType) {
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        if (propertyInfo.PropertyType == typeof(Guid) ||
                            propertyInfo.PropertyType == typeof(Guid?))
                            return new GuidGenerator();
                        break;
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = (FieldInfo)memberInfo;
                        if (fieldInfo.FieldType == typeof(Guid) ||
                            fieldInfo.FieldType == typeof(Guid?))
                            return new GuidGenerator();
                        break;
                    default:
                        break;
                }
            }
            return null;
        }

        #endregion
    }
}
