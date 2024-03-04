using System.Globalization;
using System.Resources;
using PersonDictionary.Application.Interface;

namespace PersonDictionary.Application.ResourceManagerService
{
    public class ResourceManagerService : IResourceManagerService
    {
        private readonly ResourceManager _resourceManager;

        public ResourceManagerService(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public string? GetString(string name)
        {
            return _resourceManager.GetString(name, CultureInfo.CurrentCulture);
        }
    }
}