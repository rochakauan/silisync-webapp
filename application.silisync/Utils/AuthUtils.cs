using System.Text.RegularExpressions;
using domain.silisync.Common.Results;
using domain.silisync.Common.Results.Errors;
using Microsoft.AspNetCore.Identity;

namespace application.silisync.Utils;

public static partial class AuthUtils
{
    private static readonly Regex OnlyLettersRegex =
        MyRegex();

    [GeneratedRegex("^[A-Za-z]+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    public static bool NotValid(string field)
    {
        return string.IsNullOrWhiteSpace(field) || 
               !OnlyLettersRegex.IsMatch(field);
    }

    public static async Task<Result<T, AuthError>> RetryOnDuplicateAsync<T>(
        Func<string, Task<IdentityResult>> tryCreate,
        Func<string> generateCandidate,
        Func<T> onSuccess)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var candidate = generateCandidate();
            var result = await tryCreate(candidate);

            if (result.Succeeded)
                return Result<T, AuthError>.Success(onSuccess(), "User created successfully!");

            var isUserNameDuplicated = result.Errors.Any(e =>
                e.Code == nameof(IdentityErrorDescriber.DuplicateUserName));
                
            if (!isUserNameDuplicated)
            {
                return Result<T, AuthError>.Failure(
                    AuthError.Validation(
                        result.Errors.Select(e => e.Description)
                    ),
                    "Oops! We were almost there... Please correct it and try again."
                );
            }
        }

        return Result<T, AuthError>.Failure(
            AuthError.UserTagGeneration(
                "We encountered an internal issue and had to abort the request. Please try again in a few moments. " +
                "If the error persists, please contact our support team!"
                ), 
            "[ERR-AUTH.UTILS-001]");
    }
}