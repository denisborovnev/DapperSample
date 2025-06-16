using Dapper;
using DapperSample.Entities;
using Microsoft.Data.SqlClient;

namespace DapperSample;

public class Repository
{
    public static string ConnectionString { get; set; } = null!;

    public static async Task<IEnumerable<T>> QueryAsync<T>(IEnumerable<string>? selectColumns = null, IEnumerable<IFilter>? filters = null)
    {
        filters ??= new List<IFilter>();
        selectColumns ??= new List<string>();

        var table = typeof(T).Name switch
        {
            nameof(Product) => "Products",
            nameof(User) => "Users",
            _ => throw new NotImplementedException()
        };

        var parameters = new DynamicParameters();
        var whereClauses = new List<string>();
        foreach (var filter in filters)
        {
            var (whereClause, filterParameters) = filter.Apply();
            parameters.AddDynamicParams(filterParameters);
            whereClauses.Add(whereClause);
        }

        var selectStr = selectColumns.Any() ? string.Join(", ", selectColumns) : "*";
        var whereStr = whereClauses.Any() ? string.Join(" AND ", whereClauses) : "1=1";

        var query = @$"
SELECT {selectStr} FROM {table}
WHERE {whereStr}
";

        await using var conn = new SqlConnection(ConnectionString);
        return await conn.QueryAsync<T>(query, parameters);
    }
}