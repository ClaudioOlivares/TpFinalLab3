using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class RepositorioProyecto
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;

        public RepositorioProyecto(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public int Alta(Proyecto p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Proyecto (Titulo,Genero,Status,Plataforma,FechaCreacion,IdUser,Portada,Video) " +
                    $"VALUES (@titulo,@genero,@status,@plataforma, @fechaCreacion, @idUser,@portada,@video);" +
                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@titulo", p.Titulo);
                    command.Parameters.AddWithValue("@genero", p.Genero);
                    command.Parameters.AddWithValue("@status", p.Status);
                    command.Parameters.AddWithValue("@plataforma", p.Plataforma);
                    command.Parameters.AddWithValue("@fechaCreacion", p.FechaCreacion);
                    command.Parameters.AddWithValue("@idUser", p.IdUser);
                    command.Parameters.AddWithValue("@portada", p.Portada);
                    command.Parameters.AddWithValue("@video", p.Video);






                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdProyecto= res;
                    connection.Close();
                }
            }
            return res;
        }


        public int Modificacion(Proyecto p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Proyecto SET Titulo = @titulo,Genero = @genero,Status = @status,Plataforma = @plataforma,FechaCreacion = @fechaCreacion,IdUser = @idUser,Portada = @portada , Video = @video " +
                    $"WHERE IdProyecto = @idProyecto";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@titulo", p.Titulo);
                    command.Parameters.AddWithValue("@genero", p.Genero);
                    command.Parameters.AddWithValue("@status", p.Status);
                    command.Parameters.AddWithValue("@plataforma", p.Plataforma);
                    command.Parameters.AddWithValue("@fechaCreacion", p.FechaCreacion);
                    command.Parameters.AddWithValue("@idUser", p.IdUser);
                    command.Parameters.AddWithValue("@portada", p.Portada);
                    command.Parameters.AddWithValue("@idProyecto", p.IdProyecto);
                    command.Parameters.AddWithValue("@video", p.Video);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    }
}
