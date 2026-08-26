using api.silisync.Options.Abstractions;

namespace api.silisync.Options;

public class SSyncApiOptions : IApiOptions
{
    public string JwtSecret { get; init; } = string.Empty;
    
    public static string SectionName => "SSyncAPI".ToUpperInvariant();
}