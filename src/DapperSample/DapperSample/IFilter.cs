namespace DapperSample;

public interface IFilter
{
    (string whereClause, object parameters) Apply();
}