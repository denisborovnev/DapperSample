using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperSample.Tests;

public class DatabaseHelper
{
    public static void CreateDatabaseIfNotExists()
    {
        string databaseName = new SqlConnectionStringBuilder(Repository.ConnectionString).InitialCatalog;
        string connectionString = Repository.ConnectionString;

        var masterConnectionString = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" }.ConnectionString;
        using var masterDbConnection = new SqlConnection(masterConnectionString);

        var databaseCount = masterDbConnection.QuerySingle<int>($"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'");

        if (databaseCount == 1)
        {
            return;
        }

        masterDbConnection.Execute($"CREATE DATABASE [{databaseName}]");

        using var dbConnection = new SqlConnection(connectionString);
        dbConnection.Execute("CREATE TABLE Products (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(100), Description VARCHAR(100), Price DECIMAL(18,2))");
        dbConnection.Execute("CREATE TABLE Users (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(100), Email VARCHAR(100), IsActive BIT)");

        dbConnection.Execute(@"
        INSERT INTO Products (Name, Description, Price) VALUES 
        ('Product 1', 'Description 1', 100),
        ('Product 2', 'Description 2', 200);

        INSERT INTO Users (Name, Email, IsActive) VALUES 
        ('User 1', 'user1@example.com', 1),
        ('User 2', 'user2@example.com', 0)

");
    }
}