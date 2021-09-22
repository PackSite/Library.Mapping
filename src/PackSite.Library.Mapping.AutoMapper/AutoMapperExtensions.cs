namespace PackSite.Library.Mapping.AutoMapper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutoMapper configuration extensions.
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Adds AutoMapper to application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IServiceProvider, IMapperConfigurationExpression> configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddAutoMapper(configAction, null as IEnumerable<Assembly>, serviceLifetime);
        }

        /// <summary>
        /// Adds AutoMapper to application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<ILoggerFactory, IMapperConfigurationExpression> configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddAutoMapper((provider, cfg) =>
            {
                ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                configAction(loggerFactory, cfg);

            }, null as IEnumerable<Assembly>, serviceLifetime);
        }

        /// <summary>
        /// Adds AutoMapper to application.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IServiceProvider, ILoggerFactory, IMapperConfigurationExpression> configAction, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services.AddAutoMapper((provider, cfg) =>
            {
                ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                configAction(provider, loggerFactory, cfg);

            }, null as IEnumerable<Assembly>, serviceLifetime);
        }
    }
}
