using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace sharp_net {
    public enum AgeRange {
        [AttachData(DescripByLang.Chinese, "18岁及以下")]
        [AttachData(DescripByLang.English, "18 years old or less")]
        LessThan18,

        [AttachData(DescripByLang.Chinese, "19至29岁")]
        [AttachData(DescripByLang.English, "19 years old until 29 years old")]
        From19To29,

        [AttachData(DescripByLang.Chinese, "30岁及以上")]
        [AttachData(DescripByLang.English, "30 years old or more")]
        Above29
    }

    public enum DescripByLang {
        Chinese,
        English
    }

    public enum DescripEnum {
        Chinese,
        Description
    }

    //AgeRange.GetAttachedData<string>(DescripByLang.English);

    //部署N个网站分别支持N个语言，web.config设定global_lang
    public static class zGlobal {
        public static DescripByLang GlobalLang { get; set; }
        public static string GetText(this AgeRange range) {
            return range.GetAttachedData<string>(GlobalLang);
        }
    }

    //http://blog.zhaojie.me/2009/01/attachdataextensions.html
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AttachDataAttribute : Attribute {
        public AttachDataAttribute(object key, object value) {
            this.Key = key;
            this.Value = value;
        }

        public object Key { get; private set; }

        public object Value { get; private set; }
    }

    public static class AttachDataExtensions {
        public static object GetAttachedData(this ICustomAttributeProvider provider, object key) {
            var attributes = (AttachDataAttribute[])provider.GetCustomAttributes(typeof(AttachDataAttribute), false);
            return attributes.First(a => a.Key.Equals(key)).Value;
        }

        public static T GetAttachedData<T>(
            this ICustomAttributeProvider provider, object key) {
            return (T)provider.GetAttachedData(key);
        }

        public static object GetAttachedData(this Enum value, object key) {
            return value.GetType().GetField(value.ToString()).GetAttachedData(key);
        }

        public static T GetAttachedData<T>(this Enum value, object key) {
            return (T)value.GetAttachedData(key);
        }

        public static object GetAttachedDataFromObj(this object value, object key) {
            return value.GetType().GetField(value.ToString()).GetAttachedData(key);
        }

        public static T GetAttachedDataFromObj<T>(this object value, object key) {
            return (T)value.GetAttachedDataFromObj(key);
        }
    }
}
