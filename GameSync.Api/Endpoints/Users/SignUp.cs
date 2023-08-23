using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace GameSync.Api.Endpoints.Users;

public static class SignUp
{

    public class Request : IRequestWithCredentials
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class Response
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            Include(new CredentialsValidator());
        }
    }

    public class Endpoint : Endpoint<Request, Results<BadRequestWhateverError, StatusCodeHttpResult, Ok<Response>>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfirmationEmailSender _authMailService;

        public Endpoint(UserManager<User> userManager, IConfirmationEmailSender authMailService)
        {
            _userManager = userManager;
            _authMailService = authMailService;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("sign-up");
            Group<UsersGroup>();
        }

        public override async Task<Results<BadRequestWhateverError, StatusCodeHttpResult, Ok<Response>>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var newUser = new User
            {
                Email = req.Email,
                UserName = req.UserName,
            };

            var tryCreateUser = await _userManager.CreateAsync(newUser, req.Password);

            if (!tryCreateUser.Succeeded)
            {
                return new BadRequestWhateverError(tryCreateUser.Errors);
            }

            var mailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            if (!await _authMailService.SendEmailConfirmationAsync(newUser.Email, mailToken))
            {

                // delete the newly created user
                await _userManager.DeleteAsync(newUser);

                // Send an error
                return TypedResults.StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            return TypedResults.Ok(new Response
            {
                Email = req.Email,
                UserName = req.UserName,
            });
        }

    }

}
