using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Contracts;

public interface IBookRepository
{
    void AddBook(Book book);
    Book GetBook(string isbn);
    List<Book> GetAllBooks();
}