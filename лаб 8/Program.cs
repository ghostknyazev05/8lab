using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Главный класс программы
/// </summary>
internal class Program
{
    private static void Main()
    {
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        
        Console.WriteLine("Текущая директория: " + Directory.GetCurrentDirectory());

        Console.Write("Введите путь к XML-файлу (например, books.xml): ");
        string xmlPath = Console.ReadLine() ?? string.Empty;

        Console.Write("Введите путь к бинарному файлу (например, books.bin): ");
        string binaryPath = Console.ReadLine() ?? string.Empty;

        List<Book> books = BookDatabase.LoadFromBinary(binaryPath);

        if (!books.Any())
        {
            books = BookDatabase.LoadFromXml(xmlPath);
            BookDatabase.SaveToBinary(books, binaryPath);
        }

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Просмотреть все книги");
            Console.WriteLine("2 - Добавить новую книгу");
            Console.WriteLine("3 - Удалить книгу");
            Console.WriteLine("4 - Выполнить запрос");
            Console.WriteLine("0 - Выход");

            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    BookDatabase.View(books);
                    break;
                case "2":
                    BookDatabase.Add(books, binaryPath);
                    break;
                case "3":
                    BookDatabase.Remove(books, binaryPath);
                    break;
                case "4":
                    BookDatabase.ExecuteQueries(books);
                    break;
                case "0":
                    Console.WriteLine("Завершение работы программы.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}
