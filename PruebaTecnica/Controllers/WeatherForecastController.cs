using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PruebaTecnica.Models;
using PruebaTecnica.DataAccess;
using PruebaTecnica.Interfaces;

namespace PruebaTecnica.Controllers;

[ApiController]
[Route("API")]
public class WeatherForecastController : ControllerBase
{
	private ConnectionBD _connection;

	public WeatherForecastController()
	{
		_connection = new ConnectionBD();
	}

	[HttpPost(Name = "user")]
	public ActionResult<IBasicResponse> CreateUser(User user)
	{
		using (var connection = _connection.GetConnection())
		{
			connection.Open();
			using (var command = new NpgsqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "INSERT INTO usuarios (Name, Email) VALUES (@Name, @Email)";
				command.Parameters.AddWithValue("Name", user.Name);
				command.Parameters.AddWithValue("Email", user.Email);
				command.ExecuteNonQuery();
			}
		}

		IBasicResponse data = new BasicResponse()
		{
			Message = "perro",
			Date = DateTime.Now
		};

		return Ok(data);
	}
}

