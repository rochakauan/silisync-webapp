namespace application.silisync.Dtos;

public sealed class ApplicationUserResponseDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}