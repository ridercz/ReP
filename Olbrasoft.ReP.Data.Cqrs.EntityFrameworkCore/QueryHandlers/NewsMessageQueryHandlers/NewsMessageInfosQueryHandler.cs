namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class NewsMessageInfosQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageInfosQuery, IEnumerable<NewsMessageInfoDto>>
{
    public NewsMessageInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<NewsMessageInfoDto>> GetResultToHandleAsync(NewsMessageInfosQuery query, CancellationToken token)
        => await ProjectTo<NewsMessageInfoDto>(GetOrderBy(nm => nm.Date)).ToArrayAsync(token);
}
