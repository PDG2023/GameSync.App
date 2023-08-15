using Xunit;

namespace GameSync.Api.Tests;

public class PaginatedResultTests
{
    public class UnitTests
    {

        [Fact]
        public void Empty_collection_should_produces_no_previous_or_next_urls()
        {
            var listWithoutCollection = new PaginatedResult<int>(Array.Empty<int>(), 10, 10, string.Empty);

            Assert.Null(listWithoutCollection.NextPage);
            Assert.Null(listWithoutCollection.PreviousPage);
            Assert.Empty(listWithoutCollection.Items);
        }

        [Fact]
        public void Collection_with_ten_elements_and_page_size_of_five_produces_five_items_without_a_previous_url_and_with_a_next_url()
        {
            // arrange
            const int pageSize = 5;
            const int repeatedElement = 0;

            // act
            var result = new PaginatedResult<int>(Enumerable.Repeat(repeatedElement, 10), pageSize, 0, "http://localhost/");

            // assert
            Assert.Null(result.PreviousPage);
            Assert.Equal($"http://localhost/?pageSize={pageSize}&page=1", result.NextPage);
            Assert.Equal(result.Items, Enumerable.Repeat(repeatedElement, pageSize));

        }
    }
}
