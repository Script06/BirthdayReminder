using BirthdayReminder;
using System;

class Program
{
    static void Main(string[] args)
    {
        var database = new Database("Data Source=people.db");

        while (true)
        {
            Console.WriteLine("1. Отобразить весь список ДР");
            Console.WriteLine("2. Отобразить сегодняшние и ближайшие ДР");
            Console.WriteLine("3. Добавить запись");
            Console.WriteLine("4. Удалить запись");
            Console.WriteLine("5. Редактировать запись");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllBirthdays(database);
                    break;
                case "2":
                    ShowUpcomingBirthdays(database);
                    break;
                case "3":
                    AddPerson(database);
                    break;
                case "4":
                    DeletePerson(database);
                    break;
                case "5":
                    EditPerson(database);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверная опция. Попробуйте снова.");
                    break;
            }
        }
    }

    static void ShowAllBirthdays(Database database)
    {
        Console.Clear();
        var people = database.GetAllPeople();
        foreach (var person in people)
        {
            Console.WriteLine($"{person.Id}. {person.LastName} {person.FirstName} - {person.BirthDate.ToShortDateString()}");
            Console.WriteLine("---------------------------------------------------");
        }
    }

    static void ShowUpcomingBirthdays(Database database)
    {
        Console.Clear();
        var people = database.GetUpcomingBirthdays();
        foreach (var person in people)
        {
            Console.WriteLine($"{person.Id}. {person.LastName} {person.FirstName} - {person.BirthDate.ToShortDateString()}");
            Console.WriteLine("---------------------------------------------------");
        }
        
    }

    static void AddPerson(Database database)
    {
        Console.Clear();
        Console.Write("Введите фамилию: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите дату рождения (гггг-мм-дд): ");
        var birthDate = DateTime.Parse(Console.ReadLine());

        var person = new Person { LastName = lastName, FirstName = firstName, BirthDate = birthDate };
        database.AddPerson(person);
    }

    static void DeletePerson(Database database)
    {
        Console.Clear();
        ShowAllBirthdays(database);
        Console.Write("Введите ID записи для удаления: ");
        var id = int.Parse(Console.ReadLine());
        database.DeletePerson(id);
    }

    static void EditPerson(Database database)
    {
        Console.Clear();
        ShowAllBirthdays(database);
        Console.Write("Введите ID записи для редактирования: ");
        var id = int.Parse(Console.ReadLine());

        Console.Write("Введите новую фамилию: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите новое имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите новую дату рождения (гггг-мм-дд): ");
        var birthDate = DateTime.Parse(Console.ReadLine());

        var person = new Person { Id = id, LastName = lastName, FirstName = firstName, BirthDate = birthDate };
        database.UpdatePerson(person);
    }
}
