namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore;
public class SqliteRepDbContext : RepDbContext
{

    public SqliteRepDbContext(DbContextOptions options) : base(options) { }

}

public class SqliteRepDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SqliteRepDbContext>
{
    public SqliteRepDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<RepDbContext>();
        builder.UseSqlite("Data Source=../Altairis.ReP.Web/App_Data/ReP_design.db");
        return new SqliteRepDbContext(builder.Options);
    }
}