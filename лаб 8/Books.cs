using System;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// Представляет книгу в каталоге с основными полями.
/// </summary>
[Serializable]
public class Book
{
    private int _id;
    private string _title;
    private string _author;
    private int _year;
    private double _price;
    private bool _isAvailable;

    public int Id
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("ID must be non-negative.");
            _id = value;
        }
    }

    public string Title
    {
        get => _title;
        set => _title = value ?? string.Empty;
    }

    public string Author
    {
        get => _author;
        set => _author = value ?? string.Empty;
    }

    public int Year
    {
        get => _year;
        set => _year = value;
    }

    public double Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price must be non-negative.");
            _price = value;
        }
    }

    public bool IsAvailable
    {
        get => _isAvailable;
        set => _isAvailable = value;
    }

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
        builder.Append("ID: ").Append(_id)
               .Append(", Название: ").Append(_title)
               .Append(", Автор: ").Append(_author)
               .Append(", Год: ");

        if (_year < 0)
            builder.Append(Math.Abs(_year)).Append(" до н.э.");
        else
            builder.Append(_year);

        builder.Append(", Цена: ").AppendFormat("{0:C}", _price)
               .Append(", В наличии: ").Append(_isAvailable ? "да" : "нет");

        return builder.ToString();
    }
}
