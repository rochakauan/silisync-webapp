using application.silisync.Dtos;
using application.silisync.Interfaces.Application;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;
using domain.silisync.Responses;
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

    public async Task<Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>> GetAllUsersAsync(
        GetAllUsersRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName);
            
            var skip = (request.PageNumber - 1) * request.PageSize;
            
            var users = await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            if (users.Count == 0)
                return Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>
                    .Success(
                        PagedResponse<List<ApplicationUserResponseDto>>.Empty("We have nothing to show yet."));
            
            var totalCount = await query.CountAsync(cancellationToken);

            var usersDto = users.Select(u => new ApplicationUserResponseDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                })
                .ToList();
            
            return Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>
            .Success(PagedResponse<List<ApplicationUserResponseDto>>.Paged(usersDto, totalCount));
        }
        catch (Exception ex) when (ex.InnerException is SqlException)
        {
            return Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>
                .Failure(ApplicationUsersError.SqlError(GenericErrorMessage), GenericErrorMessage);
        }
        catch (Exception)
        {
            return Result<PagedResponse<List<ApplicationUserResponseDto>>, ApplicationUsersError>
                .Failure(ApplicationUsersError.UnexpectedError(GenericErrorMessage), GenericErrorMessage);
        }
    }
}