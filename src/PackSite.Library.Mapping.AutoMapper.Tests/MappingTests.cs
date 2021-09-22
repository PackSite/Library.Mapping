namespace PackSite.Library.Mapping.AutoMapper.Tests
{
    using FluentAssertions;
    using global::AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Mapping.AutoMapper;
    using PackSite.Library.Mapping.AutoMapper.Tests.Data;
    using Xunit;

    public class MappingTests
    {
        [Fact]
        public void Should_register_mappings_and_map()
        {
            using ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddAutoMapper((provider, cfg) =>
                {
                    ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    cfg.AddMappingsFrom(loggerFactory, typeof(Destination).Assembly);
                })
                .BuildServiceProvider(true);

            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            Source source = new() { ValueSource = 13 };
            Destination destination = mapper.Map<Destination>(source);

            destination.ValueDestination.Should().Be(source.ValueSource + 1);
        }
    }
}
