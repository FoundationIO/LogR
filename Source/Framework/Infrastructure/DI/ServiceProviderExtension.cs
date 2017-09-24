using System;

namespace Framework.Infrastructure.DI
{
    public static class ServiceProviderExtension
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
