namespace Olbrasoft.Data.Cqrs.FreeSql;
public interface IConfigure<TSource>
{
    Expression<Func<TSource, TDestination>> Configure<TDestination>() where TDestination : new();
}
