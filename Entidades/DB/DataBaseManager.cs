using System;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;
        static DataBaseManager()
        {
            //Alumno:

            DataBaseManager.stringConnection = "Server=DESKTOP-FBL3OPJ\\SQLEXPRESS;Database=20230622SP;Trusted_Connection=True;";
        }

        public static string GetImagenComida(string tipoComida)
        {
            // Implementar la lógica para obtener la URL de la imagen
            // ...
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    //Alumno:

                    string querry = "SELECT imagen FROM comidas WHERE tipo_comida = @comida";

                    SqlCommand cmd = new SqlCommand(querry, DataBaseManager.connection);

                    cmd.Parameters.AddWithValue("comida", tipoComida);

                    DataBaseManager.connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }

                    throw new ComidaInvalidaExeption("Comida Inexistente\n");
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Errror al leer la base de dato\n", ex);
            }
        }
       
        public static void GuardarTicket<T>(string nombreCliente, T comida) where T : IComestible, new()
        {
            try
            {
                // Implementar la lógica para guardar el ticket en la BD
                // ...
                //Alumno:

                using (SqlConnection connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO tickets (empleado,ticket) VALUES (@empleado, @ticket)";

                        // Agregar parámetros
                        command.Parameters.AddWithValue("empleado", nombreCliente);
                        command.Parameters.AddWithValue("ticket", comida.Ticket); //nombre de la clase como tipo de comida

                        // Ejecutar la consulta SQL
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException($"Error al guardar el ticket: {ex.Message}", ex);
            }
        }
    }

}
