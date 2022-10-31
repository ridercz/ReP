namespace Olbrasoft.ReP.Data.Cqrs.FreeSql;
public interface IDbContextProxy : IDbSetProvider
{
    Task<int> SaveChangesAsync(CancellationToken token);
}
