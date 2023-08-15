using Xunit;

namespace GameSync.Api.Tests;

public class PaginatedResultTests
{
    [Fact]
    public void Empty_collection_should_produce_no_previous_or_next_urls()
    {
        var listWithoutCollection = new PaginatedResult<int>(Array.Empty<int>());

        Assert.Null(listWithoutCollection.NextPage);
        Assert.Null(listWithoutCollection.PreviousPage);
        Assert.Empty(listWithoutCollection.Items);
    }
}
