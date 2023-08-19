using FluentValidation.Results;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Reflection;

namespace GameSync.Api;

// Wrapper around a BadRequest for whatever kind of errors with a simplified interface and JSON deserialization compatible properties
public class BadRequestWhateverError : IResult, IEndpointMetadataProvider
{

    public IEnumerable<Error> Errors { get; init; }

    public BadRequestWhateverError(IEnumerable<ValidationFailure> errors) 
    {
        Errors = errors.Select(validationError => new Error(validationError));
    }

    public BadRequestWhateverError(IEnumerable<IdentityError> errors) 
    {
        Errors = errors.Select(x => new Error(x));
    }

    public BadRequestWhateverError() { Errors = new List<Error>();  }


    public Task ExecuteAsync(HttpContext httpContext)
    {
        return httpContext.Response.SendAsync(this, (int)HttpStatusCode.BadRequest);
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Metadata.Add(new {
        
            ContentTypes = new[] { "application/problem+json" },
            StatusCode = (int)HttpStatusCode.BadRequest,
            Type = typeof(BadRequestWhateverError)
        });
    }

    public class Error
    {
        public string? Code { get; init; }
        public string? Description { get; init; }

        public Error(IdentityError error)
        {
            Code = error.Code;
            Description = error.Description;
        }

        public Error(ValidationFailure failure)
        {
            Code = failure.PropertyName;
            Description = failure.ErrorMessage;
        }

        public Error () { }

    }
}
