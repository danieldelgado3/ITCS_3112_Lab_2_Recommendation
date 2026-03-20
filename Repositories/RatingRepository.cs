using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Repositories;

public class RatingRepository
{
    
    private List<Rating> _ratings = new List<Rating>();

    public void AddRating(Rating rating)
    {
        if (rating == null)
            throw new ArgumentNullException(nameof(rating));
        
        for (int i = 0; i < _ratings.Count; i++)
        {
            if (_ratings[i].Member.AccountId == rating.Member.AccountId &&
                _ratings[i].Book.ISBN == rating.Book.ISBN)
            {
                _ratings.RemoveAt(i);
                break;
            }
        }

        _ratings.Add(rating);
    }

    public Rating GetRating(string memberId, string isbn)
    {
        for (int i = 0; i < _ratings.Count; i++)
        {
            if (_ratings[i].Member.AccountId.ToString() == memberId &&
                _ratings[i].Book.ISBN == isbn)
            {
                return _ratings[i];
            }
        }

        throw new KeyNotFoundException("Rating not found");
    }

    public IReadOnlyList<Rating> GetRatingsForMember(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Invalid memberId");

        List<Rating> memberRatings = new List<Rating>();

        for (int i = 0; i < _ratings.Count; i++)
        {
            if (_ratings[i].Member.AccountId.ToString() == memberId)
            {
                memberRatings.Add(_ratings[i]);
            }
        }

        return memberRatings;
    }

    public IReadOnlyList<Rating> GetAll()
    {
        return _ratings;
    }
}