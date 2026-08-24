using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Entities;

namespace domain.silisync.Repositories;

public interface IUserRepository
{
    Task<Result<User, RepositoryError>> GetUserByEmail(string email, CancellationToken cancellationToken);
}