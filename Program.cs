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
        IAuthenticationService authService = new AuthenticationService(memberRepo);
        IRecommendationService recommendationService = new RecommendationService(ratingRepo, bookRepo, memberRepo);

        // file loaders
        BookFileLoader bookLoader = new BookFileLoader(bookRepo);
        RatingFileLoader ratingLoader = new RatingFileLoader(ratingRepo, memberRepo, bookRepo);

        Console.WriteLine("Welcome to the Book Recommendation System!");
        RunMenu(bookService, ratingService, authService, bookLoader, ratingLoader,  recommendationService);
    }

    public static void RunMenu(IBookService bookService, IRatingService ratingService, IAuthenticationService authService, BookFileLoader bookLoader, RatingFileLoader ratingLoader, IRecommendationService recommendationService)
    {
        bool running = true;

        while (running)
        {
            if (authService.IsLoggedIn())
                Console.WriteLine($"Logged in as: {authService.GetCurrentUser()!.Name} (ID: {authService.GetCurrentUser()!.AccountId})");
            else
                Console.WriteLine("Not logged in.");

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Load books from file");
            Console.WriteLine("2) Load ratings from file");
            Console.WriteLine("3) Add a new book");
            Console.WriteLine("4) Rate a book");
            Console.WriteLine("5) View my ratings");
            Console.WriteLine("6) List all books");
            Console.WriteLine("7) Login");
            Console.WriteLine("8) Logout");
            Console.WriteLine("9) View book recommendations");
            Console.WriteLine("0) Exit");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter book file path: ");
                    string bookPath = Console.ReadLine()!;
                    try
                    {
                        bookLoader.Load(bookPath);
                        Console.WriteLine("Books loaded successfully!");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"File '{bookPath}' not found.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error loading books: {e.Message}");
                    }
                    break;

                case "2":
                    Console.Write("Enter ratings file path: ");
                    string ratingsPath = Console.ReadLine()!;
                    try
                    {
                        ratingLoader.Load(ratingsPath);
                        Console.WriteLine("Ratings loaded successfully!");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"File '{ratingsPath}' not found.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error loading ratings: {e.Message}");
                    }
                    break;

                case "3":
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

                case "4":
                    if (!authService.IsLoggedIn())
                    {
                        Console.WriteLine("Please log in first.");
                        break;
                    }

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
                        string memberId = authService.GetCurrentUser()!.AccountId.ToString();
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

                case "5":
                    if (!authService.IsLoggedIn())
                    {
                        Console.WriteLine("Please log in first.");
                        break;
                    }

                    string userId = authService.GetCurrentUser()!.AccountId.ToString();
                    var ratings = ratingService.GetUserRatings(userId);

                    if (ratings.Count == 0)
                        Console.WriteLine("No ratings found.");
                    else
                        foreach (var r in ratings)
                            Console.WriteLine($"{r.Book.Title} -> {r.Value}");
                    break;

                case "6":
                    var booksList = bookService.GetAllBooks();
                    if (booksList.Count == 0)
                        Console.WriteLine("No books available.");
                    else
                        foreach (var b in booksList)
                            Console.WriteLine($"{b.ISBN} | {b.Title} by {b.Author} ({b.Year})");
                    break;

                case "7":
                    Console.Write("Enter your account ID: ");
                    string loginId = Console.ReadLine() ?? "";
                    try
                    {
                        authService.Login(loginId);
                        Console.WriteLine($"Welcome, {authService.GetCurrentUser()!.Name}!");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Login failed: {ex.Message}");
                    }
                    catch (KeyNotFoundException ex)
                    {
                        Console.WriteLine($"Login failed: {ex.Message}");
                    }
                    break;

                case "8":
                    if (authService.IsLoggedIn())
                    {
                        string name = authService.GetCurrentUser()!.Name;
                        authService.Logout();
                        Console.WriteLine($"Goodbye, {name}!");
                    }
                    else
                    {
                        Console.WriteLine("You are not logged in.");
                    }
                    break;
                
                case "9":
                    if (!authService.IsLoggedIn())
                    {
                        Console.WriteLine("Please log in first.");
                        break;
                    }

                    var currentUser = authService.GetCurrentUser()!;
                    var recommendedBooks = recommendationService.GetRecommendations(currentUser, 5);

                    if (recommendedBooks.Count == 0)
                        Console.WriteLine("No recommendations available. Rate more books to get better suggestions!");
                    else
                    {
                        Console.WriteLine("Recommended books for you:");
                        foreach (var b in recommendedBooks)
                            Console.WriteLine($"{b.ISBN} | {b.Title} by {b.Author} ({b.Year})");
                    }
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