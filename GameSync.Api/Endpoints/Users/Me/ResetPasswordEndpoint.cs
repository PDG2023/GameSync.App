namespace GameSync.Api.Endpoints.Users.Me;

public class ResetPasswordEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Group<MeGroup>();
    }
}
