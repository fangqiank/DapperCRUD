using Microsoft.Data.SqlClient;

namespace Web.Api.Services
{
    public class SqlConnectionFactory
    {
        private readonly string _conn;

        public SqlConnectionFactory(string conn)
        {
            _conn = conn;
        }
        public SqlConnection Create()
        {
            return new SqlConnection(_conn);
        } 
    }
}
