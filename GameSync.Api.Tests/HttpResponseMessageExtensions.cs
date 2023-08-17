using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
