using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

/// <summary>
/// Класс, который реализует основную логику по работе с базой книг
/// </summary>
public static class BookDatabase
{
    public static List<Book> Load(string binaryPath)
    {
        try
        {
            using FileStream stream = new FileStream(binaryPath, FileMode.Open);
            return JsonSerializer.Deserialize<List<Book>>(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении бинарного файла: {ex.Message}");
            return new List<Book>();
        }
    }

    public static List<Book> InitializeFromXml(string xmlPath, string binaryPath)
    {
        if (!File.Exists(xmlPath))
        {
            Console.WriteLine("XML-файл не найден.");
            return null;
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));

            using FileStream xmlStream = new FileStream(xmlPath, FileMode.Open);
            List<Book> books = (List<Book>)serializer.Deserialize(xmlStream);

            Save(books, binaryPath);

            Console.WriteLine("Данные загружены из XML и сохранены в бинарный файл.");
            return books;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке XML: {ex.Message}");
            return null;
        }
    }

    public static void Save(List<Book> books, string binaryPath)
    {
        using FileStream stream = new FileStream(binaryPath, FileMode.Create);
        JsonSerializer.Serialize(stream, books);
    }

    public static void View(List<Book> books)
    {
        if (!books.Any())
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        books.Select(book => book.ToString()).ToList().ForEach(Console.WriteLine);
    }

    public static void Add(List<Book> books, string binaryPath)
    {
        try
        {
            Console.Write("Вы уверены, что хотите добавить новую книгу? (Y/N): ");
            string confirm = Console.ReadLine();
            if (!string.Equals(confirm, "Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Операция отменена.");
                return;
            }

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

            Save(books, binaryPath);
            Console.WriteLine("Книга успешно добавлена.");
        }
        catch
        {
            Console.WriteLine("Ошибка ввода. Проверьте данные.");
        }
    }

    public static void Remove(List<Book> books, string binaryPath)
    {
        Console.Write("Введите ID книги для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный ID.");
            return;
        }

        Book book = books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            Console.WriteLine("Книга не найдена.");
            return;
        }

        Console.WriteLine($"Найдена книга: {book}");
        Console.Write("Вы уверены, что хотите удалить эту книгу? (Y/N): ");
        string confirm = Console.ReadLine();
        if (!string.Equals(confirm, "Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Удаление отменено.");
            return;
        }

        books.Remove(book);
        Save(books, binaryPath);
        Console.WriteLine("Книга удалена.");
    }

    public static void ExecuteQueries(List<Book> books)
    {
        Console.WriteLine("\nВыберите запрос:");
        Console.WriteLine("1 - Книги после 2000 года");
        Console.WriteLine("2 - В наличии и дешевле 500 ₽");
        Console.WriteLine("3 - Средняя цена всех книг");
        Console.WriteLine("4 - Общее количество книг");

        Console.Write("Ваш выбор: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.WriteLine("\nКниги после 2000 года:");
                books.Where(b => b.Year > 2000).ToList().ForEach(Console.WriteLine);
                break;

            case "2":
                Console.WriteLine("\nВ Доступные книги дешевле 500 ₽:");
                books.Where(b => b.IsAvailable && b.Price < 500).ToList().ForEach(Console.WriteLine);
                break;

            case "3":
                Console.WriteLine("\nСредняя цена книг:");
                Console.WriteLine($"{(books.Any() ? books.Average(b => b.Price) : 0):F2} ₽");
                break;

            case "4":
                Console.WriteLine("\nОбщее количество книг:");
                Console.WriteLine(books.Count);
                break;

            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }
}
