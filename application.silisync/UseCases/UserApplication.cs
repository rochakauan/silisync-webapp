using application.silisync.Dtos;
using application.silisync.Interfaces.Application;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using persistence.silisync.Identity;

namespace application.silisync.UseCases;

public class UserApplication(UserManager<ApplicationUser> userManager) : IUserApplication
{
    private const string GenericErrorMessage =
        "We encountered an internal issue and had to abort the request. Please try again in a few moments. " +
        "If the error persists, please contact our support team!";
    
    public async Task<Result<List<ApplicationUserResponseDto>, ApplicationUsersError>> GetAllUsersAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await userManager.Users
                .AsNoTracking().ToListAsync(cancellationToken);

            if (users.Count == 0)
                return Result<List<ApplicationUserResponseDto>, ApplicationUsersError>
                    .Failure(ApplicationUsersError.None(),
                        "We have nothing to show yet.");

            var usersDto = users.Select(u =>
                    new ApplicationUserResponseDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                    })
                .ToList();

            return Result<List<ApplicationUserResponseDto>, ApplicationUsersError>
                .Success(usersDto);
        }
        catch (Exception ex) when (ex.InnerException is SqlException)
        {
            return Result<List<ApplicationUserResponseDto>, ApplicationUsersError>
                .Failure(ApplicationUsersError.SqlError(), GenericErrorMessage);
        }
        catch (Exception)
        {
            return Result<List<ApplicationUserResponseDto>, ApplicationUsersError>
                .Failure(ApplicationUsersError.UnexpectedError(GenericErrorMessage), GenericErrorMessage);
        }
    }
}