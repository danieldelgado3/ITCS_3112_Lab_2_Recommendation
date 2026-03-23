using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;
using System;
using System.Collections.Generic;

namespace ITCS_3112_Lab_2_Recommendation.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRatingRepository _ratingRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IMemberRepository _memberRepo;

        public RecommendationService(IRatingRepository ratingRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
        {
            _ratingRepo = ratingRepo;
            _bookRepo = bookRepo;
            _memberRepo = memberRepo;
        }

        public List<Book> GetRecommendations(Member member, int topN)
        {
            var allBooks = _bookRepo.GetAllBooks();
            var memberRatings = _ratingRepo.GetRatingsForMember(member.AccountId.ToString());

            HashSet<string> ratedIsbns = new HashSet<string>();
            foreach (var r in memberRatings)
            {
                if (r.Value != 0)
                {
                    ratedIsbns.Add(r.Book.ISBN);
                }
            }

            
            List<(Book book, double score)> booksWithScore = new List<(Book book, double score)>();

            foreach (var book in allBooks)
            {
                if (ratedIsbns.Contains(book.ISBN))
                    continue;

                double score = CalculatePersonalizedScore(member, book);
                if (score != 0) 
                {
                    booksWithScore.Add((book, score));
                }
            }

            booksWithScore.Sort((a, b) => b.score.CompareTo(a.score));

       
            List<Book> topRecommendations = new List<Book>();
            int count = 0;
            foreach (var tuple in booksWithScore)
            {
                topRecommendations.Add(tuple.book);
                count++;
                if (count >= topN)
                    break;
            }

            return topRecommendations;
        }

 
        private double CalculatePersonalizedScore(Member member, Book book)
        {
            double score = 0;

            var allMembers = _memberRepo.GetAllMembers();
            var allRatings = _ratingRepo.GetAll();

            foreach (var other in allMembers)
            {
                if (other.AccountId == member.AccountId)
                    continue; 
                Rating otherRating = null;
                foreach (var r in allRatings)
                {
                    if (r.Member.AccountId == other.AccountId && r.Book.ISBN == book.ISBN)
                    {
                        otherRating = r;
                        break;
                    }
                }

                if (otherRating == null)
                    continue; 
                
                double similarity = CalculateSimilarity(member, other);
                score += similarity * (int)otherRating.Value;
            }

            return score;
        }
        
        private double CalculateSimilarity(Member m1, Member m2)
        {
            double sum = 0;

            var ratings1 = _ratingRepo.GetRatingsForMember(m1.AccountId.ToString());
            var ratings2 = _ratingRepo.GetRatingsForMember(m2.AccountId.ToString());

            foreach (var r1 in ratings1)
            {
                foreach (var r2 in ratings2)
                {
                    if (r1.Book.ISBN == r2.Book.ISBN)
                    {
                        sum += (int)r1.Value * (int)r2.Value; 
                    }
                }
            }

            return sum; 
        }
    }
}