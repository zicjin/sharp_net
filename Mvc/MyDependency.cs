using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace sharp_net.Mvc {
    public class HttpContextLife<T> : LifetimeManager, IDisposable {
        public override object GetValue() {
            return HttpContext.Current.Items[typeof(T).AssemblyQualifiedName];
        }
        public override void RemoveValue() {
            HttpContext.Current.Items.Remove(typeof(T).AssemblyQualifiedName);
        }
        public override void SetValue(object newValue) {
            HttpContext.Current.Items[typeof(T).AssemblyQualifiedName] = newValue;
        }
        public void Dispose() {
            RemoveValue();
        }
    }

    public class MyDependencyResolver : IDependencyResolver {
        private IUnityContainer container;
        public MyDependencyResolver(IUnityContainer container) {
            this.container = container;
        }
        public object GetService(Type serviceType) {
            try {
                return container.Resolve(serviceType);
            } catch {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType) {
            try {
                return container.ResolveAll(serviceType);
            } catch {
                return new List<object>();
            }
        }
    }
}
