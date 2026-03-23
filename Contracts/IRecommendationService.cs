using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IRecommendationService
{
    public List<Book> GetRecommendations(Member member, int topN);
}