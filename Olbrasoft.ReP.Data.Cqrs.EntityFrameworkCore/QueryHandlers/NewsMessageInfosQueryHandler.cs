using Altairis.ReP.Data.Dtos.NewsMessageDtos;
using Altairis.ReP.Data.Entities;
using Altairis.ReP.Data.Queries;

namespace Olbrasoft.ReP.Data.Cqrs.EntityFrameworkCore.QueryHandlers;
public class NewsMessageInfosQueryHandler : BaseQueryHandlerWithProjector<NewsMessage, NewsMessageInfosQuery, IEnumerable<NewsMessageInfoDto>>
{
    public NewsMessageInfosQueryHandler(IProjector projector, RepDbContext context) : base(projector, context)
    {
    }

    protected override async Task<IEnumerable<NewsMessageInfoDto>> GetResultToHandleAsync(NewsMessageInfosQuery query, CancellationToken token)
        => await ProjectTo<NewsMessageInfoDto>(OrderBy(nm => nm.Date)).ToArrayAsync(token);
}
