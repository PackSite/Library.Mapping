namespace PackSite.Library.Mapping.AutoMapper
{
    using System.Linq;
    using System.Reflection;
    using global::AutoMapper;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Mapping.AutoMapper.Internal;

    /// <summary>
    /// AutoMapper configuration extensions.
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
            ILogger<CustomAutoMapperProfile> logger = loggerFactory.CreateLogger<CustomAutoMapperProfile>();
            mapperConfiguration.AddProfile(new CustomAutoMapperProfile(assembly, logger));

            return mapperConfiguration;
        }

        /// <summary>
        /// Adds custom mappings from one or more assemblies.
        /// </summary>
        /// <param name="mapperConfiguration">Mapper configuration.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="assembly">Assembly.</param>
        /// <returns></returns>
        public static IMapperConfigurationExpression AddMappingsFrom(this IMapperConfigurationExpression mapperConfiguration, ILoggerFactory loggerFactory, Assembly[] assembly)
        {
            ILogger<CustomAutoMapperProfile> logger = loggerFactory.CreateLogger<CustomAutoMapperProfile>();

            mapperConfiguration.AddProfiles(assembly.Distinct().Select(x => new CustomAutoMapperProfile(x, logger)));

            return mapperConfiguration;
        }
    }
}
