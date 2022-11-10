namespace Olbrasoft.Data.Cqrs.FreeSql;

public interface IEntityToDtoConfigure<TEntity, TDto> : IConfiguration
{
    Expression<Func<TEntity, TDto>> Configure();
}
