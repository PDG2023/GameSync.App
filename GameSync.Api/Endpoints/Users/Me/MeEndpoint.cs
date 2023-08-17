namespace GameSync.Api.Endpoints.Users.Me;

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