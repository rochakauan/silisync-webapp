using System.Text.Json.Serialization;

namespace api.silisync.Controllers.Dtos;

public record MeliTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("expires_in")] int ExpiresIn,
    [property: JsonPropertyName("scope")] string Scope,
    [property: JsonPropertyName("user_id")] long UserId,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);