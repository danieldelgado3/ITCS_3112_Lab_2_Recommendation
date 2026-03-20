using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class BookService
{
    private IBookRepository _bookRepo;

    public BookService(IBookRepository repo)
    {
        _bookRepo = repo;
    }

    public void AddBook(Book book)
    {
        _bookRepo.AddBook(book);
    }

    public Book GetBook(string isbn)
    {
        return _bookRepo.GetBook(isbn);
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepo.GetAllBooks();
    }
}