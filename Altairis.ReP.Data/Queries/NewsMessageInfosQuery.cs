using Altairis.ReP.Data.Dtos.NewsMessageDtos;

namespace Altairis.ReP.Data.Queries;
public class NewsMessageInfosQuery : BaseQuery<IEnumerable<NewsMessageInfoDto>>
{
    public NewsMessageInfosQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}