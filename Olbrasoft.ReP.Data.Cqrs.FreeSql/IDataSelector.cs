namespace Olbrasoft.ReP.Data.Cqrs.FreeSql;
public interface IDataSelector
{
    ISelect<TEntity> Select<TEntity>() where TEntity : class;
}
