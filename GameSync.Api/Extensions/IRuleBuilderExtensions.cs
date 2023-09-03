using FluentValidation;
using GameSync.Api.Resources;
using System.Linq.Expressions;

namespace GameSync.Api.Extensions;

public static class IRuleBuilderExtensions
{
    public static IRuleBuilderOptions<TReq, TPrep> WithResourceError<TReq, TPrep>(
        this IRuleBuilderOptions<TReq, TPrep> builder,
        Expression<Func<string>> resourceSelector)
        
    {
        var body = resourceSelector.Body;
        var name = (body as MemberExpression ?? throw new Exception("Not a member expr")).Member.Name;

        return builder.WithErrorCode(name).WithMessage(resourceSelector.Compile().Invoke());
    }

    public static IRuleBuilderOptions<TReq, TPrep> WithObjectDoesNotExistError<TReq, TPrep>(
        this IRuleBuilderOptions<TReq, TPrep> builder)

    {
        return builder.WithResourceError(() => Resource.ObjectDoesNotExist);
    }

}
