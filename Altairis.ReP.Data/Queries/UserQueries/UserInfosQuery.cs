using Altairis.ReP.Data.Dtos;

namespace Altairis.ReP.Data.Queries.UserQueries;
public class UserInfosQuery : BaseQuery<IEnumerable<UserInfoDto>>
{
    public UserInfosQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }
}
