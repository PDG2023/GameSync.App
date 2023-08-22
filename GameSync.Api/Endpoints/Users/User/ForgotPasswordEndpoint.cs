using GameSync.Api.Endpoints.Users.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Users.Me;

public class ForgotPasswordEndpoint : Endpoint<SingleMailRequest, Results<Ok, BadRequestWhateverError>>
{
    public override void Configure()
    {
        Post("{Email}/forgot-password");
        Group<UsersGroup>();
    }

    public override async Task<Results<Ok, BadRequestWhateverError>> ExecuteAsync(SingleMailRequest req, CancellationToken ct)
    {
        return await Task.FromResult(TypedResults.Ok());
    }

}
