namespace LoginSystemApp.Models;

public class Publisher
{
    public int PublisherId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Contact { get; set; }
    public ICollection<Book>? Books { get; set; }
}
