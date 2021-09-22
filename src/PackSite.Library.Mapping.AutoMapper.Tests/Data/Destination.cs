namespace PackSite.Library.Mapping.AutoMapper.Tests.Data
{
    using global::AutoMapper;

    public class Destination : IMappable
    {
        public int ValueDestination { get; init; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Source, Destination>()
                .ForMember(dest => dest.ValueDestination, cfg => cfg.MapFrom(src => src.ValueSource + 1));
        }
    }
}
