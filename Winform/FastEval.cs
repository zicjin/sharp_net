using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web.UI;

namespace sharp_net {
    //http://blog.zhaojie.me/2009/01/dynamicpropertyaccessor-and-fasteval.html
    public static class FastEvalExtensions {
        private static DynamicPropertyAccessorCache s_cache = new DynamicPropertyAccessorCache();

        public static object FastEval(this Control control, object o, string propertyName) {
            return s_cache.GetAccessor(o.GetType(), propertyName).GetValue(o);
        }

        public static object FastEval(this TemplateControl control, string propertyName) {
            return control.FastEval(control.Page.GetDataItem(), propertyName);
        }
    }

    public class DynamicPropertyAccessor {
        private Func<object, object> m_getter;
        private DynamicMethodExecutor m_dynamicSetter;

        public DynamicPropertyAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName)) { }

        public DynamicPropertyAccessor(PropertyInfo propertyInfo) {
            // target: (object)((({TargetType})instance).{Property})

            // preparing parameter, object type
            ParameterExpression instance = Expression.Parameter(
                typeof(object), "instance");

            // ({TargetType})instance
            Expression instanceCast = Expression.Convert(
                instance, propertyInfo.ReflectedType);

            // (({TargetType})instance).{Property}
            Expression propertyAccess = Expression.Property(
                instanceCast, propertyInfo);

            // (object)((({TargetType})instance).{Property})
            UnaryExpression castPropertyValue = Expression.Convert(
                propertyAccess, typeof(object));

            // Lambda expression
            Expression<Func<object, object>> lambda =
                Expression.Lambda<Func<object, object>>(
                    castPropertyValue, instance);

            this.m_getter = lambda.Compile();

            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod != null) {
                this.m_dynamicSetter = new DynamicMethodExecutor(setMethod);
            }
        }

        public object GetValue(object o) {
            return this.m_getter(o);
        }

        public void SetValue(object o, object value) {
            if (this.m_dynamicSetter == null) {
                throw new NotSupportedException("Cannot set the property.");
            }
            this.m_dynamicSetter.Execute(o, new object[] { value });
        }
    }

    public class DynamicPropertyAccessorCache {
        private object m_mutex = new object();
        private Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>> m_cache =
            new Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>>();

        public DynamicPropertyAccessor GetAccessor(Type type, string propertyName) {
            DynamicPropertyAccessor accessor;
            Dictionary<string, DynamicPropertyAccessor> typeCache;

            if (this.m_cache.TryGetValue(type, out typeCache)) {
                if (typeCache.TryGetValue(propertyName, out accessor)) {
                    return accessor;
                }
            }

            lock (m_mutex) {
                if (!this.m_cache.ContainsKey(type)) {
                    this.m_cache[type] = new Dictionary<string, DynamicPropertyAccessor>();
                }

                accessor = new DynamicPropertyAccessor(type, propertyName);
                this.m_cache[type][propertyName] = accessor;

                return accessor;
            }
        }
    }
}