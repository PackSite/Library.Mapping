namespace PackSite.Library.AutoMapper
{
    using global::AutoMapper;

    /// <summary>
    /// Custom mapping.
    /// </summary>
    public interface ICustomMapping
    {
        /// <summary>
        /// Register mappings here.
        /// </summary>
        /// <param name="configuration"></param>
        void CreateMappings(Profile configuration);
    }
}
