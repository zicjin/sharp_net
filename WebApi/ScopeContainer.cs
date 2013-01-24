using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace sharp_net {
    public class ScopeContainer : IDependencyScope, IDependencyResolver {
        protected IUnityContainer container;

        public ScopeContainer(IUnityContainer container) {
            if (container == null) {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType) {
            if (container.IsRegistered(serviceType)) {
                return container.Resolve(serviceType);
            } else {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            if (container.IsRegistered(serviceType)) {
                return container.ResolveAll(serviceType);
            } else {
                return new List<object>();
            }
        }

        public void Dispose() {
            container.Dispose();
        }

        public IDependencyScope BeginScope() {
            var child = container.CreateChildContainer();
            return new ScopeContainer(child);
        }
    }
}
