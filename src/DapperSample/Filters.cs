using DapperSample.Entities;

namespace DapperSample;

public class UserEmailFilter(string email) : IFilter
{
    public (string whereClause, object parameters) Build()
    {
        return ($"{nameof(User.Email)} = @email", new { email });
    }
}

public class ProductNameFilter(string productName) : IFilter
{
    public (string whereClause, object parameters) Build()
    {
        return ($"{nameof(Product.Name)} = @productName", new { name = productName });
    }
}