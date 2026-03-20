namespace ITCS_3112_Lab_2_Recommendation.Domain;

public class Book
{
    public string ISBN { get; }
    public string Author { get; }
    public string Title { get; }
    public int Year { get; }

    public Book(string isbn, string author, string title, int year)
    {
        ISBN = isbn;
        Author = author;
        Title = title;
        Year = year;
    }
}