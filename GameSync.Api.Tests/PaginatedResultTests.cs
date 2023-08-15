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

        [Fact]
        public void Collection_with_ten_elements_and_page_size_of_five_produces_five_items_without_a_previous_url_and_with_a_next_url()
        {
            // arrange
            const int pageSize = 5;
            const int repeatedElement = 0;
            var items = Enumerable.Repeat(repeatedElement, 10);

            // act
            var result = new PaginatedResult<int>(items, pageSize, 0, "http://localhost/");

            // assert
            Assert.Null(result.PreviousPage);
            Assert.Equal($"http://localhost/?pageSize=5&page=1", result.NextPage);
            Assert.Equal(result.Items, Enumerable.Repeat(repeatedElement, pageSize));
        }

        [Fact]
        public void Collection_with_ten_elements_page_size_of_two_and_at_page_three_produces_both_urls()
        {
            // arrange 
            const int pageSize = 2;
            const int repeatedElement = 0;
            var items = Enumerable.Range(repeatedElement, 10);

            // act
            var result = new PaginatedResult<int>(items, pageSize, 2, "http://localhost/");

            // assert
            Assert.Equal("http://localhost/?pageSize=2&page=1", result.PreviousPage);
            Assert.Equal("http://localhost/?pageSize=2&page=3", result.NextPage);
            Assert.Equal(new[] { 4, 5 }, result.Items);
        }

    }
}
