using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Repositories;

public class BookRepository : IBookRepository
{
    private List<Book> _books = new List<Book>();

    public void AddBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (_books.Any(b => b.ISBN == book.ISBN))
            throw new InvalidOperationException($"Book with ISBN {book.ISBN} already exists");

        _books.Add(book);
    }

    public Book GetBook(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty", nameof(isbn));

        Book book = _books.FirstOrDefault(b => b.ISBN == isbn);

        if (book is null)
            throw new KeyNotFoundException($"Book with ISBN {isbn} not found");

        return book;
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }
}