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
        set => _id = value;
    }

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Author
    {
        get => _author;
        set => _author = value;
    }

    public int Year
    {
        get => _year;
        set => _year = value;
    }

    public double Price
    {
        get => _price;
        set => _price = value;
    }

    public bool IsAvailable
    {
        get => _isAvailable;
        set => _isAvailable = value;
    }

    public Book() { }

    public Book(int id, string title, string author, int year, double price, bool isAvailable)
    {
        _id = id;
        _title = title;
        _author = author;
        _year = year;
        _price = price;
        _isAvailable = isAvailable;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("ID: ").Append(_id)
               .Append(", Название: ").Append(_title)
               .Append(", Автор: ").Append(_author)
               .Append(", Год: ").Append(_year >= 0 ? _year.ToString() : $"{Math.Abs(_year)} до н.э.")
               .Append(", Цена: ").AppendFormat("{0:C}", _price)
               .Append(", В наличии: ").Append(_isAvailable ? "да" : "нет");
        return builder.ToString();
    }
}
