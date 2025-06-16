using DapperSample.Entities;
using static EasyTests.Asserts;

namespace DapperSample.Tests;

[TestClass]
public class RepositoryTests
{
    [TestInitialize]
    public void TestInitialize()
    {
        Repository.ConnectionString = "Server=localhost;Database=DapperSample;Trusted_Connection=True;TrustServerCertificate=True;";
        DatabaseHelper.CreateDatabaseIfNotExists();
    }

    [TestMethod]
    public async Task Query_all_users()
    {
        var users = await Repository.QueryAsync<User>();

        ExpectCollection(users).EquivalentTo(
            x => x.Name == "User 1" && x.Email == "user1@example.com",
            x => x.Name == "User 2" && x.Email == "user2@example.com"
        );
    }

    [TestMethod]
    public async Task Query_with_filter_by_email()
    {
        var users = await Repository.QueryAsync<User>(filters: [new UserEmailFilter("user2@example.com")]);
        
        ExpectCollection(users).EquivalentTo(
            x => x.Name == "User 2" && x.Email == "user2@example.com"
        );
    }

    [TestMethod]
    public async Task Query_only_user_emails()
    {
        var users = await Repository.QueryAsync<User>(selectColumns: [nameof(User.Id), nameof(User.Email)]);

        ExpectCollection(users).EquivalentTo(
            x => x.Name == null && x.Email == "user1@example.com",
            x => x.Name == null && x.Email == "user2@example.com"
        );
    }

    [TestMethod]
    public async Task Query_all_products()
    {
        var products = await Repository.QueryAsync<Product>();
        
        ExpectCollection(products).EquivalentTo(
            x => x.Name == "Product 1" && x.Description == "Description 1" && x.Price == 100,
            x => x.Name == "Product 2" && x.Description == "Description 2" && x.Price == 200
        );
    }
}