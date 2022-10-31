using Altairis.ReP.Data;
using Altairis.ReP.Data.Dtos.ReservationDtos;

namespace Olbrasoft.ReP.Business.Abstractions;
public interface IReservationService
{
    Task<IEnumerable<ReservationInfoDto>> GetReservationInfosAsync(int userId, CancellationToken token = default);

    Task<IEnumerable<UserReservationDto>> GetUserReservationsAsync(int userId, CancellationToken token = default);


    Task<IEnumerable<ReservationWithDesignInfoDto>> GetReservationsBetweenAsync(DateTime dateBegin, DateTime dateEnd, CancellationToken token = default);

    Task<IEnumerable<ReservationWithDesignInfoDto>> GetReservationsByAsync(int resourceId, DateTime dateBegin, CancellationToken token = default);

    Task<CommandStatus> DeleteReservationAsync(int reservationId, int userId, CancellationToken token = default);
    Task<CommandStatus> DeleteReservationAsync(int reservationId, CancellationToken token = default);


    Task<SaveReservationCommandResult> SaveAsync(DateTime dateBegin,
                                  DateTime dateEnd,
                                  int userId,
                                  int resourceId,
                                  bool system,
                                  string? comment,
                                  CancellationToken token = default);

    Task<SaveReservationCommandResult> SaveAsync(int id,
                                                 DateTime dateBegin,
                                                 DateTime dateEnd,
                                                 bool system,
                                                 string? comment,
                                                 CancellationToken token = default);

    Task<IEnumerable<ReservationFullInfoDto>> GetReservationFullInfosAsync(CancellationToken token = default);

    Task<ReservationEditDto?> GetReservationForEditOrNullAsync(int reservationId, CancellationToken token = default);
}
