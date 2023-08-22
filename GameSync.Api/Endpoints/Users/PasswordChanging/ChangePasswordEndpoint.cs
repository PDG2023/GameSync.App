using FluentValidation;
using GameSync.Api.Endpoints.Users.Me;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Users.IndividualUser;

public class ChangePasswordRequest
{
  
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Token { get; init; }

}

public class ChangePasswordRequestValidator : Validator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}

public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, Results<Ok, BadRequestWhateverError>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("change-password");
        Group<UsersGroup>();
    }

    public override async Task<Results<Ok, BadRequestWhateverError>> ExecuteAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            return await Task.FromResult(new BadRequestWhateverError(ValidationFailures));
        }



        return await  base.ExecuteAsync(req, ct);
    }
}
