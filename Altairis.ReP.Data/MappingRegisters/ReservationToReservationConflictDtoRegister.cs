using Altairis.ReP.Data.Entities;
using Mapster;

namespace Olbrasoft.Blog.Data.MappingRegisters;

public class ReservationToReservationConflictDtoRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reservation, ReservationConflictDto>().Map(rcd => rcd.UserName,  r => r.User!.UserName);
    }
}
