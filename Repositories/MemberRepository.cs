using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new();

    public Member GetOrCreateMemberByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        var existing = GetMemberByName(name);
        if (existing != null)
            return existing;

        return CreateAndAddMember(name);
    }

    private Member CreateAndAddMember(string name)
    {
        int nextId = _members.Count == 0 ? 1 : _members.Max(m => m.AccountId) + 1;
        var member = new Member(name, nextId);
        AddMember(member);
        return member;
    }

    public void AddMember(Member member)
    {
        if (member is null)
            throw new ArgumentNullException(nameof(member));

        if (_members.Any(m => m.AccountId == member.AccountId))
            throw new InvalidOperationException($"Member with account ID {member.AccountId} already exists");

        _members.Add(member);
    }

    public Member GetMember(int accountId)
    {
        var member = _members.FirstOrDefault(m => m.AccountId == accountId)
            ?? throw new KeyNotFoundException($"Member with account ID {accountId} not found");
        return member;
    }

    public Member? GetMemberByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;
        return _members.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<Member> GetAllMembers() => _members;
}
