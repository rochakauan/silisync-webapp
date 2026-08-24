namespace domain.silisync.Responses;

public class PagedResponse<T> : Response<T>
{
    private PagedResponse(
        T? data, int totalCount, int currentPage = 1,
        int pageSize = ResultsConfiguration.DefaultPageSize)
        : base(data: data)
    {
        Data = data;
        TotalCount = totalCount; 
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    
    private PagedResponse(T? data, 
        int code = ResultsConfiguration.DefaultStatusCode, 
        string? message = null)
        : base(code, data, message) { }
    
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    
    public int TotalPages => 
        (int)Math.Ceiling(TotalCount / (double)PageSize);
    
    public static PagedResponse<T> Paged(T data, int totalCount, int currentPage = 1, 
        int pageSize = ResultsConfiguration.DefaultPageSize)
        => new(data, totalCount, currentPage, pageSize);
    
    public static PagedResponse<T> Empty(string message, int code = ResultsConfiguration.DefaultStatusCode)
        => new(default, code, message);
}