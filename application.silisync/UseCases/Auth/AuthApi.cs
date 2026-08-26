using application.silisync.Interfaces.Application;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;
using domain.silisync.Responses;
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

        var publicId = Guid.CreateVersion7();
        
        var applicationUser = new ApplicationUser
        {
            Id = publicId,
            UserName = request.Username.ToLower(), 
            Email = request.Email
        };
        
        var identityResult = await userManager.CreateAsync(applicationUser, request.Password);

        if (!identityResult.Succeeded)
            return Result<Guid, AuthError>.Failure(
                AuthError.Validation(identityResult.Errors.Select(e => e.Description)),
                "Oops! We were almost there... Please correct it and try again.");
        
        // var domainUser = User.Create(publicId, model.Username);
        // TODO: persist domain user
        
        return Result<Guid, AuthError>.Success(applicationUser.Id, "User created successfully!");
    }

    public async Task<Result<string, AuthError>> LoginAsync(LoginRequest request)
    {
       var user = await userManager.FindByEmailAsync(request.Email);
       if (user is null)
           return Result<string, AuthError>.Success("Invalid credentials!");

       var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
       if (!isValidPassword)
           return Result<string, AuthError>.Success("Invalid credentials!");

       var token = TokenService.GenerateToken(user, configuration);

       return Result<string, AuthError>.Success(token, $"It's good to see you back, {user.UserName}!");
    }
}