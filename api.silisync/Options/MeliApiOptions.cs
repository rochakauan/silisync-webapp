using api.silisync.Options.Abstractions;

namespace api.silisync.Options;

public class MeliApiOptions : IApiOptions
{
    public string AppId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string RedirectUri { get; init; } = string.Empty;
    
    public static string SectionName => "MercadoLibre".ToUpperInvariant();
}