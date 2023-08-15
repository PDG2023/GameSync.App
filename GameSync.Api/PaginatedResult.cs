namespace GameSync.Api;

public class PaginatedResult<T>
{

    public PaginatedResult(IEnumerable<T> collection, int pageSize, int pageNumber, string absoluteUrlToDestination)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

        if (pageNumber < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }

        int offset = pageSize * pageNumber;
        int count = collection.Count();

        if (offset + pageSize < count) // there are items still left in the next page 
        {
            NextPage = BuildPageUrl(pageNumber + 1, pageSize, absoluteUrlToDestination);

        }

        if (pageNumber > 0 && count > 0)
        {
            PreviousPage = BuildPageUrl(pageNumber - 1, pageSize, absoluteUrlToDestination);
        }

        Items = collection.Skip(offset).Take(pageSize).ToList();

    }


    public IEnumerable<T> Items { get; set; }
    public string? NextPage { get; private set; }
    public string? PreviousPage { get; private set; }

    private string BuildPageUrl(int page, int pageSize, string absoluteUrl)
    {

        var builder = new UriBuilder(absoluteUrl);
        var begginingChar = builder.Query.Contains('?') ? "&" : "?";
        builder.Query += $"{begginingChar}pageSize={pageSize}&page={page}";
        return builder.Uri.AbsoluteUri;
    }


}
