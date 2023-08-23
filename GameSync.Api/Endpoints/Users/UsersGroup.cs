namespace GameSync.Api.Endpoints.Users;

public class UsersGroup : Group
{
    public UsersGroup() 
    { 
        Configure("users", x => {
            x.DontThrowIfValidationFails();
        }); 
    }
}
