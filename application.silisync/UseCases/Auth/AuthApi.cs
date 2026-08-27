using application.silisync.Dtos;
using application.silisync.Interfaces.Application;
using application.silisync.Utils;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using persistence.silisync.Identity;

namespace application.silisync.UseCases.Auth;

public class AuthApi(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IAuthApi
{
    public async Task<Result<Guid, AuthError>> RegisterAsync(CreateUserRequest request)
    {
        var userExists = await userManager.FindByEmailAsync(request.Email);
        if (userExists is not null)
            return Result<Guid, AuthError>.Failure(
                AuthError.EmailAlreadyInUse(request.Email),
                "It was not possible to proceed with the request due to one of the fields.");
        
        if (AuthUtils.NotValid(request.Username))
            return Result<Guid, AuthError>.Failure(
                AuthError.Validation(["The username must contain only letters, without numbers or symbols."]),
                "Invalid username");
        
        var publicId = Guid.CreateVersion7();

        var result = await AuthUtils.RetryOnDuplicateAsync(
            generateCandidate: () => $"{request.Username.ToLower()}#{Random.Shared.Next(1, 10000):D4}",
            tryCreate: async candidateUsername =>
            {
                var appUser = new ApplicationUser
                {
                    Id = publicId,
                    UserName = candidateUsername,
                    Email = request.Email
                };
                
                appUser.UserName = candidateUsername;
                return await userManager.CreateAsync(appUser, request.Password);
            },
            onSuccess: () => publicId
            );
        
        // var domainUser = User.Create(publicId, model.Username);
        // TODO: persist domain user

        return result;
    }

    public async Task<Result<LoggedDto, AuthError>> LoginAsync(LoginRequest request)
    {
       var user = await userManager.FindByEmailAsync(request.Email);
       if (user is null)
           return Result<LoggedDto, AuthError>.Failure(AuthError.InvalidCredentials(
               "Username or password is incorrect"
               ), "Invalid credentials!");

       var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
       if (!isValidPassword)
           return Result<LoggedDto, AuthError>.Failure(AuthError.InvalidCredentials(
                   "Username of password is incorrect"
                   ), 
               "Invalid credentials!");

       var token = TokenService.GenerateToken(user, configuration);

       return Result<LoggedDto, AuthError>.Success(
           new LoggedDto { UserName = user.UserName ?? "", Email = user.Email ?? "", AccessToken = token }, 
           $"It's good to see you back, {user.UserName}!");
    }
}