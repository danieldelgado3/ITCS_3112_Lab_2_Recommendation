using System.Globalization;
using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;
using ITCS_3112_Lab_2_Recommendation.Enum;

namespace ITCS_3112_Lab_2_Recommendation.Services;

/// <summary>
/// Loads ratings from file. Single responsibility: parse file format and delegate to repositories.
/// </summary>
public class RatingFileLoader : IFileLoader
{
    private readonly IRatingRepository _ratingRepo;
    private readonly IMemberRepository _memberRepo;
    private readonly IBookRepository _bookRepo;

    public RatingFileLoader(IRatingRepository ratingRepo, IMemberRepository memberRepo, IBookRepository bookRepo)
    {
        _ratingRepo = ratingRepo ?? throw new ArgumentNullException(nameof(ratingRepo));
        _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
        _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
    }

    public void Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path));

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i + 1 < lines.Length; i += 2)
        {
            string name = lines[i].Trim();
            string[] ratingStrings = lines[i + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Member member = _memberRepo.GetOrCreateMemberByName(name);

            for (int j = 0; j < ratingStrings.Length; j++)
            {
                if (!int.TryParse(ratingStrings[j], NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                    continue;

                if (!System.Enum.IsDefined(typeof(RatingValue), value))
                    continue;

                string isbn = (j + 1).ToString();
                if (!_bookRepo.TryGetBook(isbn, out Book? book) || book is null)
                    continue;

                var rating = new Rating(member, book, (RatingValue)value);
                _ratingRepo.AddRating(rating);
            }
        }
    }
}
