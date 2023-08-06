using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users;


public class ConfirmRequest
{
    [QueryParam]
    public required string ConfirmationToken { get; init; }

    [QueryParam]
    public required string Email { get; init; }
}


public class ConfirmEndpoint : Endpoint<ConfirmRequest, Results<NotFound, NoContent, BadRequestWhateverError>>
{
    private readonly UserManager<User> userManager;

    public override void Configure()
    {
        Post("confirm");
        Group<UsersGroup>();
        AllowAnonymous();
    }

    public ConfirmEndpoint(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public override async Task<Results<NotFound, NoContent, BadRequestWhateverError>> ExecuteAsync(ConfirmRequest req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var identityResult = await userManager.ConfirmEmailAsync(user, req.ConfirmationToken);

        if (!identityResult.Succeeded)
        {
            return new BadRequestWhateverError(identityResult.Errors);
        }

        return TypedResults.NoContent();
    }

}
