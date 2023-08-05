namespace GameSync.Api.Endpoints.Users;

public class UserAuthGroup : Group
{
    public UserAuthGroup() 
    { 
        Configure("users", x => {
            x.AllowAnonymous(); 
        }); 
    }
}
