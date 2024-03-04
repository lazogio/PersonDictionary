namespace PersonDictionary.Domain.GenericModel;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<T> Results { get; set; }

    public PagedResult()
    {
        Results = new List<T>();
    }
}