#nullable enable
using System.Reflection;

namespace Diner.Bootstrapping
{
    public class Registrant
    {
        public Assembly Assembly { get; }
        public Type SingletonAttribute { get; }
        public Type IgnoreDefaultConventionAttribute { get; }

        public Registrant(Assembly assembly, Type? singletonAttribute = null, Type? ignoreDefaultConventionAttribute = null)
        {
            Assembly = assembly;
            SingletonAttribute = singletonAttribute ?? typeof(SingletonAttribute);
            IgnoreDefaultConventionAttribute = ignoreDefaultConventionAttribute ?? typeof(IgnoreDefaultConventionAttribute);
        }
    }

    public readonly struct RegistryOverride
    {
        public Type Implementor { get; }
        public Type Service { get; }
        public bool AsSingleton { get; }

        public RegistryOverride(Type implementor, Type service, bool? asSingleton = null)
        {
            Implementor = implementor;
            Service = service;
            AsSingleton = asSingleton.GetValueOrDefault();
        }
    }
}
#nullable restore
