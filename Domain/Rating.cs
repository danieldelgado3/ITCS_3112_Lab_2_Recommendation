using ITCS_3112_Lab_2_Recommendation.Enum;

namespace ITCS_3112_Lab_2_Recommendation.Domain;

public class Rating
{
    public Member Member { get; }
    public Book Book { get; }
    public RatingValue Value { get; }

    public Rating(Member member, Book book, RatingValue value)
    {
        Member = member;
        Book = book;
        Value = value;
    }
}