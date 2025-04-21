using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {

        Console.WriteLine("Текущая директория: " + Directory.GetCurrentDirectory());

        if (!System.IO.File.Exists("books.dat"))
        {
            BookDatabase.InitializeFromXml();
        }

        List<Book> books = BookDatabase.Load();

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
                    BookDatabase.Add(books);
                    break;
                case "3":
                    BookDatabase.Remove(books);
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