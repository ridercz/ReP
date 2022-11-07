using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityToDtoConfigurations;
public class ReservationTo_ConflictDto : IEntityToDtoConfigure<Reservation, ReservationConflictDto>
{
    public Expression<Func<Reservation, ReservationConflictDto>> Configure()
    {
        return r => new ReservationConflictDto { UserName = r.User!.UserName };
    }
}
