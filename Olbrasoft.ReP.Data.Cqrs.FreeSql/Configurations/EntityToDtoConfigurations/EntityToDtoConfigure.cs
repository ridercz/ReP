using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityToDtoConfigurations;

public interface IEntityToDtoConfigure<TEntity, TDto> : IConfiguration
{
    Expression<Func<TEntity, TDto>> Configure();
}
