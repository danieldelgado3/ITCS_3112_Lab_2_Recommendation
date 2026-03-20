using ITCS_3112_Lab_2_Recommendation.Contracts;
using ITCS_3112_Lab_2_Recommendation.Domain;

namespace ITCS_3112_Lab_2_Recommendation.Services;

public class BookFileLoader : IFileLoader
{
    
    private readonly IBookRepository _bookRepo;
    
    public void Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }
        string[] lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string [] sections = line.Split(',');
            string author = sections[0];
            string title = sections[1];
            int year = int.Parse(sections[2]);
            string isbn = (i + 1).ToString();
            Book book = new Book(author, title, isbn, year);
            _bookRepo.AddBook(book);
        }
    }
}