using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

/// <summary>
/// Статический класс для управления каталогом книг.
/// </summary>
public static class BookDatabase
{
    private const string BinaryFilePath = "books.dat";
    private const string XmlFilePath = "books.xml";

    public static List<Book> Load()
    {
        if (!File.Exists(BinaryFilePath))
        {
            Console.WriteLine("Бинарный файл не найден.");
            return new List<Book>();
        }

        try
        {
            using FileStream stream = new FileStream(BinaryFilePath, FileMode.Open);
            return JsonSerializer.Deserialize<List<Book>>(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            return new List<Book>();
        }
    }

    public static void Save(List<Book> books)
    {
        using FileStream stream = new FileStream(BinaryFilePath, FileMode.Create);
        JsonSerializer.Serialize(stream, books);
    }

    public static void InitializeFromXml()
    {
        if (!File.Exists(XmlFilePath))
        {
            Console.WriteLine("Файл books.xml не найден.");
            return;
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));

            using FileStream stream = new FileStream(XmlFilePath, FileMode.Open);
            List<Book> books = (List<Book>)serializer.Deserialize(stream);

            Save(books);
            Console.WriteLine("База данных успешно инициализирована из XML.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке XML: {ex.Message}");
        }
    }

    public static void View(List<Book> books)
    {
        if (!books.Any())
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        foreach (Book book in books)
        {
            Console.WriteLine(book);
        }
    }

    public static void Add(List<Book> books)
    {
        try
        {
            Console.Write("Введите ID: ");
            int id = int.Parse(Console.ReadLine());

            if (books.Any(b => b.Id == id))
            {
                Console.WriteLine("Книга с таким ID уже существует.");
                return;
            }

            Console.Write("Введите название: ");
            string title = Console.ReadLine();

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();

            Console.Write("Введите год издания: ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Введите цену: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Книга в наличии (true/false): ");
            bool isAvailable = bool.Parse(Console.ReadLine());

            Book newBook = new Book(id, title, author, year, price, isAvailable);
            books.Add(newBook);
            Save(books);
            Console.WriteLine("Книга успешно добавлена.");
        }
        catch
        {
            Console.WriteLine("Ошибка при добавлении книги. Проверьте ввод.");
        }
    }

    public static void Remove(List<Book> books)
    {
        Console.Write("Введите ID книги для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный ID.");
            return;
        }

        Book bookToRemove = books.FirstOrDefault(b => b.Id == id);

        if (bookToRemove == null)
        {
            Console.WriteLine("Книга не найдена.");
            return;
        }

        books.Remove(bookToRemove);
        Save(books);
        Console.WriteLine("Книга удалена.");
    }

    public static void ExecuteQueries(List<Book> books)
    {
        Console.WriteLine("\nЗапрос 1: Книги, изданные после 2000 года:");
        var recentBooks = books.Where(b => b.Year > 2000);
        foreach (var book in recentBooks)
        {
            Console.WriteLine(book);
        }

        Console.
WriteLine("\nЗапрос 2: Книги, в наличии и дешевле 500 рублей:");
        var cheapAvailableBooks = books.Where(b => b.Price < 500 && b.IsAvailable);
        foreach (var book in cheapAvailableBooks)
        {
            Console.WriteLine(book);
        }

        Console.WriteLine("\nЗапрос 3: Средняя цена всех книг:");
        double averagePrice = books.Any() ? books.Average(b => b.Price) : 0;
        Console.WriteLine($"{averagePrice:F2} руб.");

        Console.WriteLine("\nЗапрос 4: Общее количество книг:");
        Console.WriteLine(books.Count);
    }
}