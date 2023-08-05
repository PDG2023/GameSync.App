using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Endpoints.Users;

public class SignUpRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class SucessfulSignUpResponse
{
    public required string Email { get; set; }
}

public class SignUpEndpoint : Endpoint<SignUpRequest, Results<BadRequest<IEnumerable<IdentityError>>, Ok<SucessfulSignUpResponse>>>
{
    private readonly UserManager<User> userManager;
    private readonly IUserValidator<User> validator;

    public SignUpEndpoint(UserManager<User> userManager, IUserValidator<User> validator)
    {
        this.userManager = userManager;
        this.validator = validator;
    }

    public override void Configure()
    {
        Post("sign-up");
        Group<UserAuthGroup>();
    }

    public override async Task<Results<BadRequest<IEnumerable<IdentityError>>, Ok<SucessfulSignUpResponse>>> ExecuteAsync(SignUpRequest req, CancellationToken ct)
    {
        var newUser = new User
        {
            Email = req.Email,
            UserName = req.Email
        };

        var tryCreateUser = await userManager.CreateAsync(newUser, req.Password);

        if (!tryCreateUser.Succeeded)
        {
            return TypedResults.BadRequest(tryCreateUser.Errors);
        }

        return TypedResults.Ok(new SucessfulSignUpResponse
        {
            Email = req.Email
        });
    }
}
