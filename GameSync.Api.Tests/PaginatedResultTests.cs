using Xunit;

namespace GameSync.Api.Tests;

public class PaginatedResultTests
{
    public class UnitTests
    {

        [Fact]
        public void Empty_collection_should_produces_no_previous_or_next_urls()
        {
            var listWithoutCollection = new PaginatedResult<int>(Array.Empty<int>(), 10, 10, "http://localhost/");

            Assert.Null(listWithoutCollection.NextPage);
            Assert.Null(listWithoutCollection.PreviousPage);
            Assert.Empty(listWithoutCollection.Items);
        }


        [Theory]
        [InlineData(5, 0, null, "http://localhost/?pageSize=5&page=1", 0, 1, 2, 3, 4)]
        [InlineData(2, 2, "http://localhost/?pageSize=2&page=1", "http://localhost/?pageSize=2&page=3", 4, 5)]
        [InlineData(5, 1, "http://localhost/?pageSize=5&page=0", null, 5, 6, 7, 8, 9)]
        public void Collection_with_specified_elements_produce_specified_urls_and_items(int pageSize, int page, string? expectedPreviousUrl, string? expectedNextUrl, params int[] expectedItems)
        {
            // arrange
            var items = Enumerable.Range(0, 10);

            // act
            var result = new PaginatedResult<int>(items, pageSize, page, "http://localhost/");

            // assert
            Assert.Equal(expectedPreviousUrl, result.PreviousPage);
            Assert.Equal(expectedNextUrl, result.NextPage);
            Assert.Equal(expectedItems, result.Items);
        }
  
    }
}
