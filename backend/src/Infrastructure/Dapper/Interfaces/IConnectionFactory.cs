using Microsoft.Data.SqlClient;

namespace Infrastructure.Dapper.Interfaces
{
    public interface IConnectionFactory
    {
        SqlConnection GetSqlConnection();
    }
}
