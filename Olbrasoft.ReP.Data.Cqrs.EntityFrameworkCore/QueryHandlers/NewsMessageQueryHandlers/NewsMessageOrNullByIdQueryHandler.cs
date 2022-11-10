namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers.NewsMessageQueryHandlers;
public class NewsMessageOrNullByIdQueryHandler : RepDbQueryHandler<NewsMessage, NewsMessageOrNullByIdQuery, NewsMessageDto?>
{
    public NewsMessageOrNullByIdQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<NewsMessageDto?> GetResultToHandleAsync(NewsMessageOrNullByIdQuery query, CancellationToken token)
        => await ProjectTo<NewsMessageDto>(GetWhere(nm => nm.Id == query.NewsMessageId)).SingleOrDefaultAsync(token);
}
