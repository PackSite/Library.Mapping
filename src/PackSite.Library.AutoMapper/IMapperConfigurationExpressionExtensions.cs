namespace PackSite.Library.AutoMapper
{
    using System.Reflection;
    using global::AutoMapper;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutoMapper configuration extensions.
    /// </summary>
    public static class IMapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Adds custom mappings from assembly.
        /// </summary>
        /// <param name="mapperConfiguration"></param>
        /// <param name="assembly"></param>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static IMapperConfigurationExpression AddCustomMappings(this IMapperConfigurationExpression mapperConfiguration, Assembly assembly, ILoggerFactory loggerFactory)
        {
            ILogger<CustomAutoMapperProfile> logger = loggerFactory.CreateLogger<CustomAutoMapperProfile>();
            mapperConfiguration.AddProfile(new CustomAutoMapperProfile(assembly, logger));

            return mapperConfiguration;
        }
    }
}
