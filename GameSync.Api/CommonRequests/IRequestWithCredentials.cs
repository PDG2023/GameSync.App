namespace GameSync.Api.Common;

public interface IRequestWithCredentials
{
    string Password { get; }
    string Email { get; }
}
