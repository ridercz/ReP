using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityToDtoConfigurations;
public class ResevationToUserReservationDto : IEntityToDtoConfigure<Reservation, UserReservationDto>
{
    public Expression<Func<Reservation, UserReservationDto>> Configure()
        => r => new UserReservationDto { ResourceName = r.Resource!.Name };
}
