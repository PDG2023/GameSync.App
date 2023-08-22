using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class ForgotPasswordEndpoint : Endpoint<SingleMailRequest, Results<Ok, BadRequestWhateverError>>
{

    public ForgotPasswordEndpoint(UserManager<User> manager, IForgotPasswordEmailSender sender)
    {

    }

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
