using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

/// <summary>
/// Класс для работы с базой данных книг
/// </summary>
internal class BookDatabase
{
    // Загрузка списка книг из XML-файла
    public static List<Book> LoadFromXml(string xmlPath)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
            using FileStream fs = new FileStream(xmlPath, FileMode.Open);
            return (List<Book>)serializer.Deserialize(fs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении XML-файла: {ex.Message}");
            return new List<Book>();
        }
    }

    // Загрузка списка книг из бинарного файла (JSON)
    public static List<Book> LoadFromBinary(string binaryPath)
    {
        try
        {
            if (!File.Exists(binaryPath))
            {
                return new List<Book>();
            }

            string json = File.ReadAllText(binaryPath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении бинарного файла: {ex.Message}");
            return new List<Book>();
        }
    }

    // Сохранение списка книг в бинарный файл (JSON)
    public static void SaveToBinary(List<Book> books, string binaryPath)
    {
        try
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(binaryPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в бинарный файл: {ex.Message}");
        }
    }

    // Просмотр всех книг
    public static void View(List<Book> books)
    {
        if (!books.Any())
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        StringBuilder output = new StringBuilder();

        foreach (var book in books)
        {
            output.AppendLine(book.ToString());
        }

        Console.WriteLine(output.ToString());
    }


    // Добавление новой книги
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

            int id = InputValidator.GetValidIntInput("Введите ID: ", allowNegative: false);

            if (books.Any(b => b.Id == id))
            {
                Console.WriteLine("Книга с таким ID уже существует.");
                return;
            }

            Console.Write("Введите название: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Введите автора: ");
            string author = Console.ReadLine() ?? string.Empty;

            int year = InputValidator.GetValidIntInput("Введите год издания: ");
            double price = InputValidator.GetValidDoubleInput("Введите цену: ", allowNegative: false);

            Console.Write("Книга в наличии? (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out bool isAvailable))
            {
                Console.WriteLine("Ошибка ввода наличия книги.");
                return;
            }

            Book newBook = new Book(id, title, author, year, price, isAvailable);
            books.Add(newBook);

            SaveToBinary(books, binaryPath);
            Console.WriteLine("Книга успешно добавлена.");
        }
        catch
        {
            Console.WriteLine("Ошибка ввода. Проверьте корректность данных.");
        }
    }

    // Удаление книги по ID
    public static void Remove(List<Book> books, string binaryPath)
    {
        Console.Write("Введите ID книги для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Неверный формат ID.");
            return;
        }

        Book book = books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            Console.WriteLine("Книга с таким ID не найдена.");
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
        SaveToBinary(books, binaryPath);
        Console.WriteLine("Книга успешно удалена.");
    }

    // Выполнение пользовательских запросов
    public static void ExecuteQueries(List<Book> books)
    {
        Console.WriteLine("\nВыберите запрос:");
        Console.WriteLine("1 - Книги после 2000 года");
        Console.WriteLine("2 - Книги в наличии и дешевле 500 ₽");
        Console.WriteLine("3 - Средняя цена всех книг");
        Console.WriteLine("4 - Общее количество книг");

        Console.Write("Ваш выбор: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                var booksAfter2000 = books.Where(b => b.Year > 2000).OrderBy(b => b.Year);
                Console.WriteLine("\nКниги после 2000 года:");
                foreach (var book in booksAfter2000)
                {
                    Console.WriteLine(book);
                }
                return;

            case "2":
                var availableCheapBooks = books.Where(b => b.IsAvailable && b.Price < 500);
                Console.WriteLine("\nКниги в наличии и дешевле 500 ₽:");
                foreach (var book in availableCheapBooks)
                {
                    Console.WriteLine(book);
                }
                return;

            case "3":
                double averagePrice = books.Any() ? books.Average(b => b.Price) : 0;
                Console.WriteLine($"\nСредняя цена всех книг: {averagePrice:F2} ₽");
                return;

            case "4":
                Console.WriteLine($"\nОбщее количество книг: {books.Count}");
                return;

            default:
                Console.WriteLine("Неверный выбор запроса.");
                return;
        }
    }
}
