using application.silisync.Interfaces.Application;
using persistence.silisync.Identity;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using domain.silisync.Requests.Users;
using Microsoft.AspNetCore.Identity;

namespace application.silisync.UseCases;

public class AuthApi(UserManager<ApplicationUser> userManager) : IAuthApi
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
        
        return Result<Guid, AuthError>.Success(applicationUser.Id);
    }

}