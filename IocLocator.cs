using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace sharp_net {
    
    /// <summary>
    /// Represents the Service Locator.
    /// </summary>
    public sealed class IocLocator {

        private IocLocator() {
            //使用Web.config配置：
            //UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //section.Configure(container);
        }

        private static IUnityContainer container;

        public static IUnityContainer Container {
            get { return container ?? (container = new UnityContainer()); }
        }

        private static IocLocator instance;

        public static IocLocator Instance {
            get { return instance ?? (instance = new IocLocator()); }
        }

        private IEnumerable<ParameterOverride> GetParameterOverrides(object overridedArguments) {
            List<ParameterOverride> overrides = new List<ParameterOverride>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property => {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });
            return overrides;
        }

        public T GetImple<T>() {
            return Container.Resolve<T>();
        }

        public T GetImple<T>(string name) {
            return Container.Resolve<T>(name);
        }

        public T GetImple<T>(object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve<T>(overrides.ToArray());
        }

        public T GetImple<T>(string name, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve<T>(name, overrides.ToArray());
        }

        public object GetImple(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object GetImple(string name, Type serviceType) {
            return Container.Resolve(serviceType, name);
        }

        public object GetImple(Type serviceType, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve(serviceType, overrides.ToArray());
        }

        public object GetImple(string name, Type serviceType, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve(serviceType, name, overrides.ToArray());
        }

        #region IDependencyResolver
        public object GetService(Type serviceType) {
            try {
                return Container.Resolve(serviceType);
            } catch {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType) {
            try {
                return Container.ResolveAll(serviceType);
            } catch {
                return new List<object>();
            }
        }
        #endregion

    }
}