namespace Olbrasoft.Data.Cqrs.FreeSql;
public class ProjectionConfigurator<TSource> : IConfigure<TSource>
{
    private readonly TryCreateConfiguration _tryCreateConfiguration;

    public ProjectionConfigurator(TryCreateConfiguration tryCreateConfiguration)
    {
        _tryCreateConfiguration = tryCreateConfiguration ?? throw new ArgumentNullException(nameof(tryCreateConfiguration));
    }

    public Expression<Func<TSource, TDestination>> Configure<TDestination>() where TDestination : new()
        => GetConfiguration<TSource, TDestination>().Configure();

    private IEntityToDtoConfigure<TEntity, TDto> GetConfiguration<TEntity, TDto>() where TDto : new()
    {
        var configurationType = typeof(IEntityToDtoConfigure<,>).MakeGenericType(typeof(TEntity), typeof(TDto));

        if (_tryCreateConfiguration(configurationType, out var configuration) && configuration is not null)
            return (IEntityToDtoConfigure<TEntity, TDto>)configuration;

        return new EntityToDtoConfiguration<TEntity, TDto>();
    }

    class EntityToDtoConfiguration<TEntity, TDto> : IEntityToDtoConfigure<TEntity, TDto> where TDto : new()
    {
        public Expression<Func<TEntity, TDto>> Configure() => e => new TDto();
    }
}
