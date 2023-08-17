using System.Reflection;
using Autofac;
using CommunityToolkit.Maui.Views;

namespace Diner.Bootstrapping
{
    public static class BootstrappingExtensions
    {
        public static Type GetViewModelTypeForView(Type viewType)
        {
            try
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void BootstrapTypes(this ContainerBuilder builder, Registrant[] registrants, RegistryOverride[] overrides = null)
        {
            try
            {
                var assemblies = registrants.Select(_ => _.Assembly).ToArray();

                builder.RegisterAssemblyModules(assemblies);
                builder.RegisterAssemblyTypesWithDefaultConvention(registrants);
                builder.RegisterViewAndViewModels(assemblies);

                // make sure overrides happen last
                builder.RegisterOverrides(overrides ?? Array.Empty<RegistryOverride>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void RegisterOverrides(this ContainerBuilder builder, RegistryOverride[] overrides)
        {
            try
            {
                var assemblies = overrides.Select(_ => _.Implementor.Assembly).Distinct().ToArray();

                builder.RegisterAssemblyTypes(assemblies)
                    .Where(t => IsOverridden(t))
                    .Where(t => !AsSingleton(t))
                    .Where(type => LogRegistration(type, false))
                    .As(t => GetOverrideInterfaces(t));

                builder.RegisterAssemblyTypes(assemblies)
                    .Where(t => IsOverridden(t))
                    .Where(t => AsSingleton(t))
                    .Where(type => LogRegistration(type, true))
                    .As(t => GetOverrideInterfaces(t))
                    .SingleInstance();

                bool IsOverridden(Type t)
                {
                    return overrides.Any(o => o.Implementor == t);
                }

                bool AsSingleton(Type t)
                {
                    return overrides.First(o => o.Implementor == t).AsSingleton;
                }

                IEnumerable<Type> GetOverrideInterfaces(Type t)
                {
                    var interfaceName = overrides.First(o => o.Implementor == t).Service.Name;
                    return t.GetInterfaces().Where(i => i.Name == interfaceName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void RegisterViewAndViewModels(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            try
            {
                builder.RegisterAssemblyTypes(assemblies)
                    .Where(t => IsView(t) || IsViewModel(t))
                    .As(t => new[] { t });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static bool IsViewModel(Type t)
        {
            var test = t.Name.EndsWith("ViewModel");
            return test;
        }

        private static bool IsView(Type t)
        {
            var test = (t.IsAssignableTo<Page>() || t.IsAssignableTo<Popup>()) && t.Name.EndsWith("Page");
            return test;
        }

        private static void RegisterAssemblyTypesWithDefaultConvention(this ContainerBuilder builder, Registrant[] registrants)
        {
            try
            {
                var assemblies = registrants.Select(_ => _.Assembly).ToArray();

                builder.RegisterAssemblyTypes(assemblies)
                    .Where((t) => !HasSingletonAttribute(t, registrants))
                    .Where(type => MatchesDefaultConvention(type, registrants))
                    .Where(type => LogRegistration(type, false))
                    .As(t => GetDefaultConventionInterfaces(t)); // AppCenter Bug: Do not simplify this to .As(GetDefaultConventionInterfaces)

                builder.RegisterAssemblyTypes(assemblies)
                    .Where(type => HasSingletonAttribute(type, registrants))
                    .Where(type => MatchesDefaultConvention(type, registrants))
                    .Where(type => LogRegistration(type, true))
                    .As(t => GetDefaultConventionInterfaces(t)) // AppCenter Bug: Do not simplify this to .As(GetDefaultConventionInterfaces)
                    .SingleInstance();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static bool LogRegistration(Type type, bool isSingleton = false)
        {
            //System.Diagnostics.Debug.WriteLine($"{type.FullName} {(isSingleton ? "Singleton" : "")}");
            return true;
        }

        private static bool MatchesDefaultConvention(Type type, Registrant[] registrants)
        {
            var registrantEntry = registrants.FirstOrDefault(_ => _.Assembly == type.Assembly);
            if (registrantEntry == null)
                return GetDefaultConventionInterfaces(type).Any();

            var hasIgnore = HasAttributeType(type, registrantEntry.IgnoreDefaultConventionAttribute);
            if (hasIgnore)
                return false;

            return GetDefaultConventionInterfaces(type).Any();
        }

        private static bool HasSingletonAttribute(Type type, Registrant[] registrants)
        {
            var registrantEntry = registrants.FirstOrDefault(_ => _.Assembly == type.Assembly);
            if (registrantEntry == null)
                return type.GetCustomAttributes<SingletonAttribute>().Any();

            var hasSingleton = HasAttributeType(type, registrantEntry.SingletonAttribute);
            return hasSingleton;
        }

        private static bool HasAttributeType(Type type, Type attType)
        {
            return type.GetCustomAttributes(attType).Any();
        }

        private static IEnumerable<Type> GetDefaultConventionInterfaces(Type type)
        {
            return type.GetInterfaces().Where(i => i.Name == $"I{type.Name}");
        }
    }
}
