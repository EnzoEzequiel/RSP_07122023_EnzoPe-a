using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    public static class FileManager
    {
        private static string path;

        static FileManager()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "20231207_Alumn");

            ValidaExistenciaDeDirectorio();
        }

        private static void ValidaExistenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException("Error al crear el directorio", ex);
            }
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            string filePath = Path.Combine(path, nombreArchivo);

            try
            {
                if (append)
                {
                    File.AppendAllText(filePath, data);
                }
                else
                {
                    File.WriteAllText(filePath, data);
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException("Error al guardar el archivo", ex);
            }
        }

        public static bool Serializar<T>(T elemento, string nombreArchivo)
        {
            string filePath = Path.Combine(path, nombreArchivo);

            try
            {
                string jsonData = JsonSerializer.Serialize(elemento);
                File.WriteAllText(filePath, jsonData);
                return true;
            }
            catch (Exception ex)
            {
                throw new FileManagerException("Error al serializar", ex);
            }
        }
    }
}
