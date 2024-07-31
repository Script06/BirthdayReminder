using System;
using System.Collections.Generic;
using System.Data;
using BirthdayReminder;
using Dapper;
using Microsoft.Data.Sqlite;

public class Database
{
    private string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS People (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    LastName TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    BirthDate TEXT NOT NULL
                )
            ");
        }
    }

    public IEnumerable<Person> GetAllPeople()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            return connection.Query<Person>("SELECT * FROM People");
        }
    }

    public IEnumerable<Person> GetUpcomingBirthdays()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var today = DateTime.Today;
            var nextWeek = today.AddDays(7);
            return connection.Query<Person>(
                "SELECT * FROM People WHERE strftime('%m-%d', BirthDate) BETWEEN strftime('%m-%d', @Today) AND strftime('%m-%d', @NextWeek)",
                new { Today = today, NextWeek = nextWeek });
        }
    }

    public void AddPerson(Person person)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute("INSERT INTO People (LastName, FirstName, BirthDate) VALUES (@LastName, @FirstName, @BirthDate)", person);
        }
    }

    public void UpdatePerson(Person person)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute("UPDATE People SET LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate WHERE Id = @Id", person);
        }
    }

    public void DeletePerson(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute("DELETE FROM People WHERE Id = @Id", new { Id = id });
        }
    }
}
