public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 8;
    public string? SearchQuery { get; set; } = "";
    public string? SortBy { get; set; } = "title";
    public string? SortOrder { get; set; } = "asc";

}
