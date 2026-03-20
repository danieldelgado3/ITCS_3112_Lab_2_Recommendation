using ITCS_3112_Lab_2_Recommendation.Domain;
using ITCS_3112_Lab_2_Recommendation.Enum;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IRatingService
{
    void RateBook(string memberId, string isbn, RatingValue value);
    IReadOnlyList<Rating> GetUserRatings(string memberId);
}