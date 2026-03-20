using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepo;

    public MemberService(IMemberRepository memberRepo)
    {
        _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
    }

    public void AddMember(Member member) => _memberRepo.AddMember(member);

    public Member GetMember(int accountId) => _memberRepo.GetMember(accountId);

    public Member? GetMemberByName(string name) => _memberRepo.GetMemberByName(name);

    public Member GetOrCreateMemberByName(string name) => _memberRepo.GetOrCreateMemberByName(name);

    public List<Member> GetAllMembers() => _memberRepo.GetAllMembers();
}
