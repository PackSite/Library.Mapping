namespace PackSite.Library.Mapping.AutoMapper
{
    using System.Linq;
    using System.Reflection;
    using global::AutoMapper;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Mapping.AutoMapper.Internal;

    /// <summary>
    /// AutoMapper mapping configuration extensions.
    /// </summary>
    public static class IMapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Adds custom mappings from assembly.
        /// </summary>
        /// <param name="mapperConfiguration">Mapper configuration.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="assembly">Assembly.</param>
        /// <returns></returns>
        public static IMapperConfigurationExpression AddMappingsFrom(this IMapperConfigurationExpression mapperConfiguration, ILoggerFactory loggerFactory, Assembly assembly)
        {
            ILogger<MappingProfile> logger = loggerFactory.CreateLogger<MappingProfile>();
            mapperConfiguration.AddProfile(new MappingProfile(assembly, logger));

            return mapperConfiguration;
        }

        /// <summary>
        /// Adds custom mappings from one or more assemblies.
        /// </summary>
        /// <param name="mapperConfiguration">Mapper configuration.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="assemblies">Assemblies.</param>
        /// <returns></returns>
        public static IMapperConfigurationExpression AddMappingsFrom(this IMapperConfigurationExpression mapperConfiguration, ILoggerFactory loggerFactory, Assembly[] assemblies)
        {
            ILogger<MappingProfile> logger = loggerFactory.CreateLogger<MappingProfile>();

            mapperConfiguration.AddProfiles(assemblies.Distinct().Select(x => new MappingProfile(x, logger)));

            return mapperConfiguration;
        }
    }
}
