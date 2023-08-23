using System.Text.Json.Serialization;

namespace GameSync.Api.CommonRequests;

public class SingleMailRequest
{

    [JsonConstructor]
    public SingleMailRequest(string email)
    {
        Email = email;
    }

    public string Email { get; init; }
}
