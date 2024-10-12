namespace BlogSharp.Data.Interfaces
{
    public interface IAspnetUser
    {
        string Name { get; }
        string Action { get; }
        string GetUserId();
        string GetUserRole();
        string GetUserEmail();
    }
}
