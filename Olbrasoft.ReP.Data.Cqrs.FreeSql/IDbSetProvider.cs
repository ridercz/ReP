namespace Olbrasoft.ReP.Data.Cqrs.FreeSql;

public interface IDbSetProvider
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}