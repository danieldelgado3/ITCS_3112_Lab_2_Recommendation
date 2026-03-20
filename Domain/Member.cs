namespace ITCS_3112_Lab_2_Recommendation.Domain;

public class Member
{
    public string Name { get; }
    public int AccountId { get; }

    public Member(string name, int accountId)
    {
        Name = name;
        AccountId = accountId;
    }
}