namespace Olbrasoft.Data.Cqrs.FreeSql;
public interface IDataSelector
{
    ISelect<TEntity> Select<TEntity>() where TEntity : class;
}
