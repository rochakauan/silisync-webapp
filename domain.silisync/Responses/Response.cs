using System.Text.Json.Serialization;

namespace domain.silisync.Responses;

public class Response<T>(
    int code = ResultsConfiguration.DefaultStatusCode,
    T? data = default, 
    string? message = null,
    IEnumerable<string>? errors = null) 
{
    [JsonConstructor]
    public Response() : this(code: ResultsConfiguration.DefaultStatusCode) { }
    
    public T? Data { get; init; } = data;
    public string? Message { get; init; } = message;
    public IEnumerable<string>? Errors { get; init; } = errors;
    
    [JsonIgnore]
    public bool IsSuccess => code is >= 200 and <= 299;
    [JsonIgnore]
    public int Code => code;
}