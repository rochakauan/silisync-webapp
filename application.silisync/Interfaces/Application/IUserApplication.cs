using application.silisync.Dtos;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;

namespace application.silisync.Interfaces.Application;

public interface IUserApplication
{
    Task<Result<List<ApplicationUserResponseDto>, ApplicationUsersError>> GetAllUsersAsync(
        CancellationToken cancellationToken = default);
}