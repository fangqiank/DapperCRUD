using Dapper;
using Web.Api.Models;
using Web.Api.Services;

namespace Web.Api.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder) 
        {
            var group = builder.MapGroup("customers");

            group.MapGet("", async (SqlConnectionFactory sqlConnection) =>
            {
                using var connection = sqlConnection.Create();

                const string sql = "SELECT Id, FirstName, LastName, Email, Dob FROM CUSTOMERS";

                var customers = await connection.QueryAsync<Customer>(sql);

                return Results.Ok(customers);
            });

            group.MapGet("{id}", async (int id, SqlConnectionFactory sqlConnection) =>
            {
                using var connection = sqlConnection.Create();

                const string sql = """
                    SELECT Id, FirstName, LastName, Email, Dob 
                    FROM CUSTOMERS
                    WHERE Id = @CustomerId
                    """;

                var customer = await connection.QuerySingleOrDefaultAsync<Customer>(
                    sql,
                    new {CustomerId = id}
                    );

                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            });

            group.MapPost("", async (Customer newCustomer, SqlConnectionFactory sqlConnection) =>
            {
                using var connection = sqlConnection.Create();

                const string sql = """
                    INSERT INTO CUSTOMERS(FirstName, LastName, Email, Dob)
                    VALUES(@FirstName, @LastName, @Email, @Dob)
                """;

                await connection.ExecuteAsync(sql, newCustomer);

                return Results.Ok(newCustomer);
            });

            group.MapPut("{id}", async (int id, Customer updCustomer, SqlConnectionFactory sqlConnection) =>
            {
                using var connection = sqlConnection.Create();

                updCustomer.Id = id;

                const string sql = """
                    UPDATE CUSTOMERS
                    SET FirstName = @FirstName, 
                        LastName = @LastName, 
                        Email = @Email, 
                        Dob = @Dob
                    WHERE Id = @Id
                """;

                await connection.ExecuteAsync(sql, updCustomer);

                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, SqlConnectionFactory sqlConnection) =>
            {
                using var connection = sqlConnection.Create();

                const string sql = """
                    DELETE FROM CUSTOMERS
                    
                    WHERE Id = @CustomerId
                """;

                await connection.ExecuteAsync(sql, new { CustomerId = id});

                return Results.NoContent();
            });
        }
    }
}
