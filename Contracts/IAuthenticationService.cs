using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IAuthenticationService
{
    void Login(string id);
    void Logout();
    bool IsLoggedIn();
    Member? GetCurrentUser();
}
