using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessAndDumpBodyIfNot(this HttpResponseMessage message, ITestOutputHelper output)
        {
            try
            {
                Assert.Equal(System.Net.HttpStatusCode.OK, message.StatusCode);
            }
            catch
            {

                output.WriteLine(await message.Content.ReadAsStringAsync());
                throw;
            }
        }
    }
}
