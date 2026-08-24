using application.silisync.Dtos;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;
using domain.silisync.Responses;

namespace application.silisync.Interfaces.Application;

public interface IUserApplication
{
    Task<Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>> GetAllUsersAsync(
        GetAllUsersRequest request,
        CancellationToken cancellationToken = default);
}