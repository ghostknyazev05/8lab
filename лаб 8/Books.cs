using System;
using System.Text;

/// <summary>
/// Представляет книгу в каталоге с основными полями.
/// </summary>
[Serializable]
public class Book
{
    public int Id { get; set; } // Уникальный идентификатор
    public string Title { get; set; } // Название книги
    public string Author { get; set; } // Автор книги
    public int Year { get; set; } // Год издания
    public double Price { get; set; } // Цена
    public bool IsAvailable { get; set; } // В наличии ли книга

    public Book() { }

    public Book(int id, string title, string author, int year, double price, bool isAvailable)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
        Price = price;
        IsAvailable = isAvailable;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("ID: ").Append(Id)
               .Append(", Название: ").Append(Title)
               .Append(", Автор: ").Append(Author)
               .Append(", Год: ").Append(Year)
               .Append(", Цена: ").AppendFormat("{0:C}", Price)
               .Append(", В наличии: ").Append(IsAvailable ? "да" : "нет");
        return builder.ToString();
    }
}