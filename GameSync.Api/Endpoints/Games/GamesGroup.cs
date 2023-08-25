namespace GameSync.Api.Endpoints.Games;

public class GamesGroup : Group
{
    public const string Prefix = "games";

    public GamesGroup()
    {
        Configure(Prefix, x =>
        {
            x.AllowAnonymous();
        });
    }
}
