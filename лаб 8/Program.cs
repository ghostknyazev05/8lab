using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Класс для взаимодействия с пользователем
/// </summary>
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Текущая директория: " + Directory.GetCurrentDirectory());

        Console.Write("Введите путь к XML-файлу (например, books.xml): ");
        string xmlFile = Console.ReadLine();

        Console.Write("Введите путь к бинарному файлу (например, books.bin): ");
        string binaryFile = Console.ReadLine();

        List<Book> books = File.Exists(binaryFile)
            ? BookDatabase.Load(binaryFile)
            : BookDatabase.InitializeFromXml(xmlFile, binaryFile);

        if (books == null)
        {
            Console.WriteLine("Ошибка загрузки данных.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n--- Каталог книг ---");
            Console.WriteLine("1. Просмотреть книги");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Выполнить запросы");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    BookDatabase.View(books);
                    break;
                case "2":
                    BookDatabase.Add(books, binaryFile);
                    break;
                case "3":
                    BookDatabase.Remove(books, binaryFile);
                    break;
                case "4":
                    BookDatabase.ExecuteQueries(books);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}
