namespace DataLayer;

public interface IAutentication
{
    Task<int> Login(string email, string password);
    Task<int> Register(string email, string password, string userName);
}
