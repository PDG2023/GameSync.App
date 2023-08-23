namespace GameSync.Api.CommonRequests;

public interface IRequestWithCredentials
{
    string Password { get; }
    string Email { get; }
}
