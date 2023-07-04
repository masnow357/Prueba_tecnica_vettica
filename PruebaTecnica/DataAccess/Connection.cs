using Npgsql;

namespace PruebaTecnica.DataAccess
{
	public class ConnectionBD
	{
		private const string ConnectionString =
			"Server=localhost;Port=5432;Database=mydatabase;User Id=postgres;Password=example;";

		public NpgsqlConnection GetConnection()
		{
			return new NpgsqlConnection(ConnectionString);
		}
	}
}

