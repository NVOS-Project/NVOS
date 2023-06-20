using System;

namespace NVOS.Core.Services
{
    public static class ServiceLocator
    {
        private static IServiceResolver resolver;

        private static void AssertResolverAvailable()
        {
            if (resolver == null)
                throw new InvalidOperationException("No service resolver is set.");
        }

        public static IServiceResolver GetResolver()
        {
            return resolver;
        }

        public static void SetResolver(IServiceResolver serviceResolver)
        {
            resolver = serviceResolver;
        }

        public static T Resolve<T>() where T : class
        {
            AssertResolverAvailable();
            return resolver.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            AssertResolverAvailable();
            return resolver.Resolve(type);
        }

        public static T ResolveOptional<T>() where T : class
        {
            AssertResolverAvailable();
            return resolver.ResolveOptional<T>();
        }

        public static object ResolveOptional(Type type)
        {
            AssertResolverAvailable();
            return resolver.ResolveOptional(type);
        }
    }
}
