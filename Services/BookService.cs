using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class BookService : IBookService
{
    private IBookRepository _bookRepo;

    public BookService(IBookRepository repo)
    {
        _bookRepo = repo;
    }

    public Boolean AddBook(Book book)
    {
        try
        {
            _bookRepo.AddBook(book);
            return true;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
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