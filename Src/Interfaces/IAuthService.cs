namespace api_pim.Interfaces;
public interface IAuthService
{
    string Authenticate(string email, string senha);
}