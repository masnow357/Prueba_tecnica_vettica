using System;
using Npgsql;
using PruebaTecnica.Models;
using PruebaTecnica.DataAccess;

namespace PruebaTecnica.Services
{
	public class UserService
	{
		private ConnectionBD _connection;

		public UserService()
		{
			_connection = new ConnectionBD();
		}

		public void CreateUser(User user)
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
		}

		/*
		public void ModificarUsuario(Usuario usuario)
		{
			using (var conexion = _connection.ObtenerConexion())
			{
				conexion.Open();
				using (var comando = new NpgsqlCommand())
				{
					comando.Connection = conexion;
					comando.CommandText = "UPDATE usuarios SET nombre = @nombre, correo_electronico = @correo WHERE id = @id";
					comando.Parameters.AddWithValue("nombre", usuario.Nombre);
					comando.Parameters.AddWithValue("correo", usuario.CorreoElectronico);
					comando.Parameters.AddWithValue("id", usuario.Id);
					comando.ExecuteNonQuery();
				}
			}
		}
		*/
	}
}

