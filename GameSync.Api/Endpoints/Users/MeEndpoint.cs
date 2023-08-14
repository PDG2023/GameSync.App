namespace GameSync.Api.Endpoints.Users;

public class MeResult
{

}


public class MeEndpoint : EndpointWithoutRequest<MeResult>
{

    public override void Configure()
    {
        Get("me");
        Claims("userid");
        Group<UsersGroup>();
    }

    public override Task<MeResult> ExecuteAsync(CancellationToken ct)
    {
        return Task.FromResult(new MeResult());
    }
}