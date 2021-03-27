namespace PackSite.Library.Mapping.AutoMapper
{
    using global::AutoMapper;

    /// <summary>
    /// Represents a mapable object.
    /// Mappable class must have a public or private parameterless constructor.
    /// </summary>
    public interface IMappable
    {
        /// <summary>
        /// Register mappings here.
        /// Mappable class must have a public or private parameterless constructor.
        /// </summary>
        /// <param name="configuration">Mapping profile.</param>
        void CreateMappings(Profile configuration);
    }
}
