# Library.Mapping

[![CI](https://github.com/PackSite/Library.Mapping/actions/workflows/CI.yml/badge.svg)](https://github.com/PackSite/Library.Mapping/actions/workflows/CI.yml)
[![Coverage](https://codecov.io/gh/PackSite/Library.Mapping/branch/main/graph/badge.svg?token=L0VTCLOWG2)](https://codecov.io/gh/PackSite/Library.Mapping)

AutoMapper extensions library

## Example configuration

Defining mappings example (with MediatR):

```
public sealed class CreateCategoryCommand
{
    public string? Name { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;

    void ICustomMapping.CreateMappings(Profile configuration)
    {
        configuration.CreateMap<CreateCategoryCommand, Category>();
    }

    private sealed class Handler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCategoryCommand query, CancellationToken cancellationToken)
        {
            (...)

            return createdEntity.Id;
        }
    }
}
```

Creating profile and registering mappings automatically:

```
public static IServiceCollection AddApplication(this IServiceCollection services, , ILoggerFactory loggerFactory)
{
    services.AddAutoMapper(cfg =>
    {
        cfg.AddCustomMappings(typeof(Application.DependencyInjection).Assembly, loggerFactory);
    });
}
```
