using GHM.Infrastructure.IServices;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace GHM.Infrastructure.Services
{
    public class ResourceService<T> : IResourceService<T> where T : class
    {
        private readonly IStringLocalizer _localizer;

        public ResourceService(IStringLocalizerFactory factory)
        {
            var type = typeof(T);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type.Name, assemblyName.Name);
        }

        public string GetString(string key)
        {
            return _localizer.GetString(key);
        }

        public string GetString(string key, params object[] arguments)
        {
            return _localizer.GetString(key, arguments);
        }
    }
}
