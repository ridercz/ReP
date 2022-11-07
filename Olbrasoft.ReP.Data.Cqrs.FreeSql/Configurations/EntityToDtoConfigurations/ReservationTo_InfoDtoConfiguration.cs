using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityToDtoConfigurations;
public class ReservationTo_InfoDtoConfiguration : IEntityToDtoConfigure<Reservation, ReservationInfoDto>
{
    public Expression<Func<Reservation, ReservationInfoDto>> Configure()
        => r => new ReservationInfoDto { ResourceName = r.Resource!.Name };
}
