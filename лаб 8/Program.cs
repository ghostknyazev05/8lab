using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Главный класс программы
/// </summary>
internal class Program
{
    private static void Main()
    {
        try
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

                if (choice == "1")
                {
                    BookDatabase.View(books);
                    continue;
                }

                if (choice == "2")
                {
                    BookDatabase.Add(books, binaryPath);
                    continue;
                }

                if (choice == "3")
                {
                    BookDatabase.Remove(books, binaryPath);
                    continue;
                }

                if (choice == "4")
                {
                    BookDatabase.ExecuteQueries(books);
                    continue;
                }

                if (choice == "0")
                {
                    Console.WriteLine("Завершение работы программы.");
                    return;
                }

                Console.WriteLine("Неверный выбор. Попробуйте снова.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
    }
}
