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
    internal class MappingProfile : Profile
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes an instance of <see cref="MappingProfile"/>.
        /// </summary>
        public MappingProfile(Assembly assembly, ILogger logger)
        {
            _logger = logger;
            LoadCustomMappings(assembly);
        }

        private void LoadCustomMappings(Assembly rootAssembly)
        {
            IReadOnlyList<Type> types = GetCustomMappings(rootAssembly).ToList();

            if (types.Count == 0)
            {
                Log.NoMappableTypes(_logger, rootAssembly);

                return;
            }

            int count = 0;
            bool anyEmpty = false;
            IEnumerable<ITypeMapConfiguration> typeMapConfigs = (this as IProfileConfiguration).TypeMapConfigs;

            Log.RegisteringMappings(_logger, types.Count, rootAssembly, types);

            foreach (Type type in types)
            {
                IMappable? map = Activator.CreateInstance(type, true) as IMappable;
                map?.CreateMappings(this);

                int v = typeMapConfigs.Count();
                if (v == count)
                {
                    anyEmpty = true;

                    Log.NoMappingsForType(_logger, type);
                }
                count = v;
            }

            Log.RegisteredMappings(_logger, typeMapConfigs.Count(), types.Count, rootAssembly);

            if (anyEmpty)
            {
                Log.AtLeastOneEmptyDefinition(_logger, rootAssembly);
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

        private static class Log
        {
            private static readonly Action<ILogger, Assembly, Exception> _noMappableTypes =
                LoggerMessage.Define<Assembly>(
                    LogLevel.Warning,
                    new EventId(1, "NoMappableTypes"),
                    $"No mappable ({typeof(IMappable).FullName}) types in {{Assembly}}. Skipping...");

            private static readonly Action<ILogger, int, Assembly, IReadOnlyList<Type>, Exception> _registeringMappings =
                LoggerMessage.Define<int, Assembly, IReadOnlyList<Type>>(
                    LogLevel.Debug,
                    new EventId(2, "RegisteringMappings"),
                    $"Registering mappings for {{Count}} types implementing '{typeof(IMappable).FullName}' in assembly '{{Assembly}}': {{Types}}");

            private static readonly Action<ILogger, Type, Exception> _noMappingsInType =
                LoggerMessage.Define<Type>(
                    LogLevel.Warning,
                    new EventId(3, "NoMappingsForType"),
                    "{Type} does not contain any mapping definitions");

            private static readonly Action<ILogger, int, int, Assembly, Exception> _registeredMappings =
                LoggerMessage.Define<int, int, Assembly>(
                    LogLevel.Information,
                    new EventId(4, "RegisteredMappings"),
                    $"Registered {{Count}} mappingss from {{MappableTypesCount}} classes implementing '{typeof(IMappable).FullName}' in {{Assembly}}");

            private static readonly Action<ILogger, Assembly, Exception> _atLeastOneEmptyDefinition =
                LoggerMessage.Define<Assembly>(
                    LogLevel.Warning,
                    new EventId(5, "AtLeastOneEmptyDefinition"),
                    $"At least one class implementing '{typeof(IMappable).FullName}' does not contain any mapping definitions in {{Assembly}}");

            public static void NoMappableTypes(ILogger logger, Assembly assembly)
            {
                _noMappableTypes(logger, assembly, null!);
            }

            public static void RegisteringMappings(ILogger logger, int count, Assembly assembly, IReadOnlyList<Type> types)
            {
                _registeringMappings(logger, count, assembly, types, null!);
            }

            public static void NoMappingsForType(ILogger logger, Type type)
            {
                _noMappingsInType(logger, type, null!);
            }

            public static void RegisteredMappings(ILogger logger, int count, int mappableTypesCount, Assembly assembly)
            {
                _registeredMappings(logger, count, mappableTypesCount, assembly, null!);
            }

            public static void AtLeastOneEmptyDefinition(ILogger logger, Assembly assembly)
            {
                _atLeastOneEmptyDefinition(logger, assembly, null!);
            }
        }
    }
}
