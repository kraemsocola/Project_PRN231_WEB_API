namespace BussinessObjects.Paging;

public class PagingRequestBase
{
    public string? KeyWords { get; set; }
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = 10;
    public int Skip => (Page - 1) * ItemsPerPage;
    public int Take { get => ItemsPerPage; }
    public string? SortField { get; set; }
}