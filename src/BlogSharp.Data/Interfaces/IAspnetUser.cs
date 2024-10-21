namespace BlogSharp.Data.Interfaces
{
    public interface IAspnetUser
    {
        string GetUserId();
        string GetUserRole();
        string GetUserEmail();
        bool IsAuthenticated();
    }
}
