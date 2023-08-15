namespace GameSync.Api;

public class PaginatedResult<T>
{

    public PaginatedResult(IEnumerable<T> collection)
    {
        Items = collection;
    }


    public IEnumerable<T> Items { get; set; }
    public string? NextPage { get; private set; }
    public string? PreviousPage { get; private set; }


}
