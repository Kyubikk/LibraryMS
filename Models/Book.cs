namespace LoginSystemApp.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public int PublisherId { get; set; }
    public Publisher? Publisher { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int YearPublished { get; set; }

    public string? ISBN { get; set; }

    public int Quantity { get; set; }

    public int Available { get; set; }

    public string? CoverImagePath { get; set; }
}
