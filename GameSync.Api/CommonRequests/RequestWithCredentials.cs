namespace GameSync.Api.CommonRequests;

public class RequestWithCredentials : RequestToUser
{
    public required string Password { get; init; }
}
