using System.Text.Json.Serialization;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class SingleMailRequest
{

    [JsonConstructor]
    public SingleMailRequest(string email)
    {
        Email = email;
    }

    public string Email { get; init; }
}
