using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;
using ITCS_3112_Lab_2_Recommendation.Enum;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class RatingService : IRatingService
{
    private IRatingRepository ratingRepo;
    private IBookRepository bookRepo;
    private IMemberRepository memberRepo;

    public RatingService(IRatingRepository rRepo, IBookRepository bRepo, IMemberRepository mRepo)
    {
        ratingRepo = rRepo;
        bookRepo = bRepo;
        memberRepo = mRepo;
    }

    public void RateBook(string memberId, string isbn, RatingValue value)
    {
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Invalid memberId");

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("Invalid ISBN");
        
        Member member = memberRepo.GetMember(int.Parse(memberId));
        Book book = bookRepo.GetBook(isbn);
        ;

        Rating rating = new Rating(member, book, value);

        ratingRepo.AddRating(rating);
    }

    public IReadOnlyList<Rating> GetUserRatings(string memberId)
    {
        return ratingRepo.GetRatingsForMember(memberId);
    }
}