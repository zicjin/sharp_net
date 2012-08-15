using System;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace zic_dotnet.Repositories.MongoDB {
    /// <summary>
    /// Represents the Bson serialization convention that serializes the <see cref="System.DateTime"/> value
    /// by using the local date/time kind.
    /// </summary>
    public class UseLocalDateTimeConvention : ISerializationOptionsConvention {
        #region ISerializationOptionsConvention Members
        /// <summary>
        /// Gets the BSON serialization options for a member.
        /// </summary>
        /// <param name="memberInfo">The member.</param>
        /// <returns>The BSON serialization options for the member; or null to use defaults.</returns>
        public virtual IBsonSerializationOptions GetSerializationOptions(MemberInfo memberInfo) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                    if (propertyInfo.PropertyType == typeof(DateTime) ||
                        propertyInfo.PropertyType == typeof(DateTime?))
                        return new DateTimeSerializationOptions(DateTimeKind.Local);
                    break;
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)memberInfo;
                    if (fieldInfo.FieldType == typeof(DateTime) ||
                        fieldInfo.FieldType == typeof(DateTime?))
                        return new DateTimeSerializationOptions(DateTimeKind.Local);
                    break;
                default:
                    break;
            }
            return null;
        }

        #endregion
    }
}
