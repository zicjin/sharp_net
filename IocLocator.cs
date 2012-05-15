using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace zic_dotnet {
    /// <summary>
    /// Represents the Service Locator.
    /// </summary>
    public sealed class IocLocator : IServiceProvider {
        
        private IocLocator() {
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

        public T GetService<T>() {
            return Container.Resolve<T>();
        }
        public T GetService<T>(string name) {
            return Container.Resolve<T>(name);
        }
        public T GetService<T>(object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve<T>(overrides.ToArray());
        }
        public T GetService<T>(string name, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve<T>(name, overrides.ToArray());
        }

        public object GetService(Type serviceType) {
            return Container.Resolve(serviceType);
        }
        public object GetService(string name, Type serviceType) {
            return Container.Resolve(serviceType, name);
        }
        public object GetService(Type serviceType, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve(serviceType, overrides.ToArray());
        }
        public object GetService(string name, Type serviceType, object overridedArguments) {
            var overrides = GetParameterOverrides(overridedArguments);
            return Container.Resolve(serviceType, name, overrides.ToArray());
        }

    }
}
