using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Entities;
using domain.silisync.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace persistence.silisync.Data.Repositories;

public class UserRepository(
    AppDbContext context,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<Result<User, RepositoryError>> GetUserByEmail(string name, CancellationToken cancellationToken)
    {
        try
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Name == name, cancellationToken);

            if (user is null)
                return Result<User, RepositoryError>.Failure(
                    RepositoryError.NotFound(name),
                    "I don't think they're here...");

            return Result<User, RepositoryError>.Success(user);
        }
        catch (Exception ex) when (ex.InnerException is SqlException)
        {
            logger.LogError(ex, "Native database error");
            return Result<User, RepositoryError>.Failure(
                RepositoryError.Critical(ex.Message),
                "Oops! We're experiencing some internal instability! We apologize. " +
                "This service will be back up in a few moments!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred while retrieving user data");
            return Result<User, RepositoryError>.Failure(
                RepositoryError.Unexpected(ex.Message),
                "An unexpected issue has occurred, and we had to abort the request. " +
                "Please try again in a few moments. If the error persists, please contact our support team!");
        }
    }
}