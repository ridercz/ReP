using System.Linq.Expressions;

namespace Olbrasoft.ReP.Data.Cqrs.FreeSql.Configurations.EntityToDtoConfigurations;
public class ReservationTo_WithDesignDtoFreeSqlMapping : IEntityToDtoConfigure<Reservation, ReservationWithDesignInfoDto>
{
    public Expression<Func<Reservation, ReservationWithDesignInfoDto>> Configure()
    {
        return r => new ReservationWithDesignInfoDto
        {
            ResourceForegroundColor = r.Resource!.ForegroundColor,
            ResourceBackgroundColor = r.Resource!.BackgroundColor,
            UserDisplayName = r.User!.DisplayName
        };
    }
}
