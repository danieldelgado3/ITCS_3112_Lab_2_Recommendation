using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;
using ITCS_3112_Lab_2_Recommendation.Enum;
using ITCS_3112_Lab_2_Recommendation.Repositories;
using ITCS_3112_Lab_2_Recommendation.Services;

namespace ITCS_3112_Lab_2_Recommendation;

class Program
{
    static void Main(string[] args)
    {
        // repositories
        IBookRepository bookRepo = new BookRepository();
        IRatingRepository ratingRepo = new RatingRepository();
        IMemberRepository memberRepo = new MemberRepository();

        // services
        IBookService bookService = new BookService(bookRepo);
        IRatingService ratingService = new RatingService(ratingRepo, bookRepo, memberRepo);

        // fileloaders
        BookFileLoader loader = new BookFileLoader(bookRepo);

        Console.WriteLine("Welcome to the Book Recommendation System!");
        RunMenu(bookService, ratingService, loader);
    }

    public static void RunMenu(IBookService bookService, IRatingService ratingService, BookFileLoader loader) // TODO: maybe add the rating file loader to constructor?
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Load books from file");
            Console.WriteLine("2) Add a new book");
            Console.WriteLine("3) Rate a book");
            Console.WriteLine("4) View my ratings");
            Console.WriteLine("5) List all books");
            Console.WriteLine("0) Exit");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter book file path: ");
                    string fileName = Console.ReadLine()!;
                    try
                    {
                        loader.Load(fileName); 
                        Console.WriteLine("Books loaded successfully!");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"File '{fileName}' not found.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error loading books: {e.Message}");
                    }
                    break;

                case "2":
                    Console.Write("Title: ");
                    string title = Console.ReadLine()!;
                    Console.Write("Author: ");
                    string author = Console.ReadLine()!;
                    Console.Write("Year: ");
                    string year = Console.ReadLine()!;
                    Console.Write("IBSN: ");
                    string isbn = Console.ReadLine()!;


                    Book book = new Book(isbn, author, title, year);
                    Boolean isBookAdded = bookService.AddBook(book);
                    if (isBookAdded)
                    {
                        Console.WriteLine("Book added successfully!");
                    }
                    break;

                case "3":
                    Console.Write("Your Member ID: ");
                    string memberId = Console.ReadLine()!;

                    Console.Write("Book ID: ");
                    string bookId = Console.ReadLine()!;

                    Console.Write("Rating (-5, -3, 0, 1, 3, 5): ");
                    string ratingInput = Console.ReadLine()!;
    
                    int intValue;
                    bool isNumber = int.TryParse(ratingInput, out intValue);

                    if (!isNumber)
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        break;
                    }

                    RatingValue ratingValue = (RatingValue)intValue;

                    try
                    {
                        ratingService.RateBook(memberId, bookId, ratingValue);
                        Console.WriteLine("Rating saved!");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unexpected error: " + ex.Message);
                    }

                    break;

                case "4":
                    Console.Write("Your Member ID: ");
                    string userId = Console.ReadLine()!;
                    var ratings = ratingService.GetUserRatings(userId);

                    if (ratings.Count == 0)
                        Console.WriteLine("No ratings found.");
                    else
                        foreach (var r in ratings)
                            Console.WriteLine($"{r.Book.Title} -> {r.Value}");
                    break;

                case "5":
                    var booksList = bookService.GetAllBooks();
                    if (booksList.Count == 0)
                        Console.WriteLine("No books available.");
                    else
                        foreach (var b in booksList)
                            Console.WriteLine($"{b.ISBN} | {b.Title} by {b.Author} ({b.Year})");
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine("Goodbye!");
    }
}