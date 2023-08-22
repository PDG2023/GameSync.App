namespace GameSync.Api.Endpoints.Users.PasswordReset;

public class PasswordResetGroup : SubGroup<UsersGroup>
{
    public PasswordResetGroup()
    {
        Configure(string.Empty, configure =>
        {
            configure.AllowAnonymous();
            configure.DontAutoTag();
            configure.Options(b => b.WithTags("Password reset"));
        });
    }
}
