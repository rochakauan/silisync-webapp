using System.Text.Json;
using api.silisync.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace api.silisync.Controllers.Api.Auth;

[ApiController]
[Route("v1/api/auth/meli-[controller]")]
public class Callback(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCallback([FromQuery] string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest(new { error = "Codigo de autorizacao nao enviado." });

        try
        {
            var appId = configuration["MercadoLibre:AppId"];
            var clientSecret = configuration["MercadoLibre:ClientSecret"];
            var redirectUri = configuration["MercadoLibre:RedirectUri"];

            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.mercadolibre.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "SiliSyncApp/beta (contact@silisync.com)");

            var payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", appId!),
                new KeyValuePair<string, string>("client_secret", clientSecret!),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri!)
            });

            var response = await client.PostAsync("oauth/token", payload);
            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new
                {
                    error = "Falha ao obter token no Mercado Livre",
                    details = jsonString
                });

            var tokenData = JsonSerializer.Deserialize<MeliTokenResponse>(jsonString);

            return Ok(new
            {
                message = "Token gerado com sucesso!",
                userId = tokenData?.UserId,
                accessToken = tokenData?.AccessToken,
                expiresIn = tokenData?.ExpiresIn
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Erro interno ao processar o callback",
                details = ex.Message
            });
        }
    }
}