using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IMemberService
{
    void AddMember(Member member);
    Member GetMember(int accountId);
    Member? GetMemberByName(string name);
    Member GetOrCreateMemberByName(string name);
    List<Member> GetAllMembers();
}
