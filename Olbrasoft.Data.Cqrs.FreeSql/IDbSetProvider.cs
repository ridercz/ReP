namespace Olbrasoft.Data.Cqrs.FreeSql;

public interface IDbSetProvider
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}