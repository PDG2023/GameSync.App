using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using GameSync.Api.CommonRequests;

namespace GameSync.Api.Endpoints.Users;

public static class SignIn
{

    public class Response
    {
        public required string Email { get; init; }
        public required string Token { get; init; }
        public required string UserName { get; init; }
    }


    public class Endpoint : Endpoint<RequestWithCredentials, Results<Ok<Response>, BadRequestWhateverError>>
    {
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration config;

        public Endpoint(SignInManager<User> signInManager, IConfiguration config)
        {
            this.signInManager = signInManager;
            this.config = config;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("sign-in");
            Group<UsersGroup>();

        }

        public override async Task<Results<Ok<Response>, BadRequestWhateverError>> ExecuteAsync(RequestWithCredentials req, CancellationToken ct)
        {

            var user = await signInManager.UserManager.FindByEmailAsync(req.Email);
            if (user is null)
            {
                AddNotFoundCredentialsErrors();
                return new BadRequestWhateverError(ValidationFailures);
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    // TODO : Translate
                    AddError(r => r.Email, "The email needs to be confirmed", "ConfirmationNeeded");
                }
                else
                {
                    AddNotFoundCredentialsErrors();
                }
                return new BadRequestWhateverError(ValidationFailures);
            }


            var token = JWTBearer.CreateToken(
                signingKey: config["Jwt:SignKey"]!,
                issuer: config["Jwt:Issuer"]!,
                audience: config["Jwt:Issuer"]!,

                expireAt: DateTime.UtcNow.AddDays(1),
                priviledges: u => {
                    u.Claims.Add((ClaimsTypes.UserId, user.Id));
                });

            var response = new Response
            {
                Email = user.Email,
                Token = token,
                UserName = user.UserName
            };
            return TypedResults.Ok(response);
        }

        private void AddNotFoundCredentialsErrors()
        {
            // TODO : Translate
            AddError(r => r.Email, "Nothing has been found for the given credentials", "NotFound");
        }
    }

}
