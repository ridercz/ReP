using Altairis.ReP.Data.Dtos;
using Altairis.ReP.Data.Entities;
using Mapster;

namespace Olbrasoft.Blog.Data.MappingRegisters;

public class ApplicationUserToRecipientDtoRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser,RecipientDto>().Map(rd=>rd.Name, ap=>ap.UserName);
    }
}