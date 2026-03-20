using ITCS_3112_Lab_2_Recommendation.Domain;
using ITCS_3112_Lab_2_Recommendation.Enum;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IRatingRepository
{
    void AddRating(Rating rating);
    Rating GetRating(string memberId, string isbn);
    IReadOnlyList<Rating> GetRatingsForMember(string memberId);
    IReadOnlyList<Rating> GetAll();
}