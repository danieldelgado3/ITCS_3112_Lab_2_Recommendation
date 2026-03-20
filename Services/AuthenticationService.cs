using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class AuthenticationService : IAuthenticationService
{
    private Member? _currentUser;
    private readonly IMemberRepository _memberRepo;

    public AuthenticationService(IMemberRepository memberRepo)
    {
        _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
    }

    public void Login(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID cannot be empty", nameof(id));

        if (!int.TryParse(id, out int accountId))
            throw new ArgumentException("Invalid account ID format", nameof(id));

        _currentUser = _memberRepo.GetMember(accountId);
    }

    public void Logout()
    {
        _currentUser = null;
    }

    public bool IsLoggedIn() => _currentUser != null;

    public Member? GetCurrentUser() => _currentUser;
}
