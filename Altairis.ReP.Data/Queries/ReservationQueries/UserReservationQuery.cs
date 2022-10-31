namespace Altairis.ReP.Data.Queries.ReservationQueries;
public class UserReservationQuery : BaseQuery<IEnumerable<UserReservationDto>>
{
    public UserReservationQuery(IDispatcher dispatcher) : base(dispatcher)
    {
    }

    public int UserId { get; set; }
}
