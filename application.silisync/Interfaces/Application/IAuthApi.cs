using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;

namespace application.silisync.Interfaces.Application;

public interface IAuthApi
{
    Task<Result<Guid, AuthError>> RegisterAsync(CreateUserRequest request);
}