using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XPeriencia.Services
{
    public static class DataManager<T>
    {
        private static string GetFilePath(string fileName) => $"Data/{fileName}.json";

        public static List<T> Load(string fileName)
        {
            var path = GetFilePath(fileName);
            if(!File.Exists(path)) return new List<T>();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

        }

        public static void Save(string fileName, List<T> data)
        {
            var path = GetFilePath(fileName);
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
