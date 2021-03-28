namespace PackSite.Library.Mapping.AutoMapper.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::AutoMapper;
    using global::AutoMapper.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Custom AutoMapper profile with mapping loader.
    /// </summary>
    internal class CustomAutoMapperProfile : Profile
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes an instance of <see cref="CustomAutoMapperProfile"/>.
        /// </summary>
        public CustomAutoMapperProfile(Assembly assembly, ILogger logger)
        {
            _logger = logger;
            LoadCustomMappings(assembly);
        }

        private void LoadCustomMappings(Assembly rootAssembly)
        {
            IReadOnlyList<Type> types = GetCustomMappings(rootAssembly).ToList();

            if (types.Count == 0)
            {
                _logger.LogWarning("No mappable ({Interface}) types in {Assembly}. Skipping...", typeof(IMappable).FullName, rootAssembly);
                return;
            }

            int count = 0;
            bool anyEmpty = false;
            IEnumerable<ITypeMapConfiguration> typeMapConfigs = (this as IProfileConfiguration).TypeMapConfigs;

            _logger.LogDebug("Registering mappings for {Count} types implementing {Interface} in {Assembly} {Types}", types.Count, typeof(IMappable).FullName, rootAssembly, types);

            foreach (Type type in types)
            {
                IMappable? map = Activator.CreateInstance(type, true) as IMappable;
                map?.CreateMappings(this);

                int v = typeMapConfigs.Count();
                if (v == count)
                {
                    anyEmpty = true;
                    _logger.LogWarning("{Type} does not contain any mapping definitions", type);
                }
                count = v;
            }

            _logger.LogInformation("Registered {Count} maps from {MappableTypesCount} classes implementing {Interface} for {Assembly}", typeMapConfigs.Count(), types.Count, typeof(IMappable).FullName, rootAssembly);

            if (anyEmpty)
            {
                _logger.LogWarning("At least one class implementing {Interface} does not contain any mapping definitions in {Assembly}", typeof(IMappable).FullName, rootAssembly);
            }
        }

        private static IEnumerable<Type> GetCustomMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetTypes();

            IEnumerable<Type>? withCustomMappings = from type in types
                                                    where typeof(IMappable).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
                                                    select type;

            return withCustomMappings.Distinct();
        }
    }
}
