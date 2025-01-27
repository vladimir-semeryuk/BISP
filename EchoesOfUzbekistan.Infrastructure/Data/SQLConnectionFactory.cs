using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Application.Abstractions.Data;

namespace EchoesOfUzbekistan.Infrastructure.Data;
internal class SQLConnectionFactory : ISQLConnectionFactory
{
    private readonly string _connectionString;

    public SQLConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetDbConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
