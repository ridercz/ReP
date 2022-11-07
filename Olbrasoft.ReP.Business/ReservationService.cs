using Altairis.ReP.Data;
using Altairis.ReP.Data.Commands.ReservationCommands;
using Altairis.ReP.Data.Dtos.ReservationDtos;
using Altairis.Services.DateProvider;
using NetBox.Extensions;

namespace Olbrasoft.ReP.Business;
public class ReservationService : BaseService, IReservationService
{
    private readonly IDateProvider _dateProvider;

    public ReservationService(IDateProvider dateProvider, IDispatcher dispatcher) : base(dispatcher)
    {
        _dateProvider = dateProvider;
    }

    public async Task<CommandStatus> DeleteReservationAsync(int reservationId, int userId, CancellationToken token = default)
        => await new DeleteReservationCommand(Dispatcher)
        {
            ResevationId = reservationId,
            UserId = userId,
            Now = _dateProvider.Now,

        }.ToResultAsync(token);

    public async Task<IEnumerable<ReservationInfoDto>> GetReservationInfosAsync(int userId, CancellationToken token = default)
    {
        var query = new ReservationsByUserIdAndDateEndQuery(Dispatcher)
        {
            UserId = userId,
            DateEndToday = _dateProvider.Today,
            Now = _dateProvider.Now

        };

       return (await query.ToResultAsync(token)).ForEach(r => r.CanBeDeleted = r.DateEnd > query.Now);
    }

    public async Task<IEnumerable<ReservationWithDesignInfoDto>> GetBetweenDatesAsync(DateTime dateBegin,
                                                                                             DateTime dateEnd,
                                                                                             CancellationToken token = default)
        => await new ReservationsBetweenDatesQuery(Dispatcher)
        {
            DateBegin = dateBegin,
            DateEnd = dateEnd

        }.ToResultAsync(token);

    public async Task<IEnumerable<ReservationWithDesignInfoDto>> GetByResourceIdAsync(int resourceId,
                                                                                                         DateTime dateBegin,
                                                                                                         CancellationToken token = default)
        => await new ReservationsByResourceIdAndDateBeginQuery(Dispatcher)
        {
            ResourceId = resourceId,
            DateBegin = dateBegin

        }.ToResultAsync(token);


    public async Task<SaveReservationCommandResult> SaveAsync(DateTime dateBegin, DateTime dateEnd, int userId, int resourceId, bool system, string? comment, CancellationToken token = default)
    {
        var result = await new InsertReservationCommand(Dispatcher)
        {
            DateBegin = dateBegin,
            DateEnd = dateEnd,
            UserId = userId,
            ResourceId = resourceId,
            System = system,
            Comment = comment

        }.ToResultAsync(token);

        ThrowExceptionIfStatusIsError(result.Status);

        return result;
    }

    public async Task<SaveReservationCommandResult> SaveAsync(int id, int resourceId, DateTime dateBegin, DateTime dateEnd, bool system, string? comment, CancellationToken token = default)
    {
        var result = await new UpdateReservationCommand(Dispatcher)
        {
            Id = id,
            ResourceId = resourceId,
            DateBegin = dateBegin,
            DateEnd = dateEnd,
            System = system,
            Comment = comment

        }.ToResultAsync(token);

        ThrowExceptionIfStatusIsError(result.Status);

        return result;
    }

    public async Task<IEnumerable<UserReservationDto>> GetUserReservationsAsync(int userId, CancellationToken token = default)
        => await new UserReservationQuery(Dispatcher) { UserId = userId }.ToResultAsync(token);

    public async Task<IEnumerable<ReservationFullInfoDto>> GetReservationFullInfosAsync(CancellationToken token = default)
        => await new ReservationFullInfosQuery(Dispatcher).ToResultAsync(token);

    public async Task<ReservationEditDto?> GetReservationForEditOrNullAsync(int reservationId, CancellationToken token = default)
        => await new ReservationEditQuery(Dispatcher) { ReservationId = reservationId }.ToResultAsync(token);

    public async Task<CommandStatus> DeleteReservationAsync(int reservationId, CancellationToken token = default)
      => await new DeleteReservationCommand(Dispatcher)
      {
          ResevationId = reservationId

      }.ToResultAsync(token);
}
