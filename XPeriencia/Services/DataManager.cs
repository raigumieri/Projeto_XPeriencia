using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XPeriencia.Services
{
    /// <summary>
    /// Classe genérica para gerenciar leietura e escrita de dados em arquivos JSON.
    /// Permite carregar e salvar listas de qualquer tipo de objeto.
    /// </summary>
    /// <typeparam name="T">Tipo de objeto a ser armazenado (ex: Usuario, Aposta, Reflexao).</typeparam>
    public static class DataManager<T>
    {
        /// <summary>
        /// Retorna o caminho completo do arquivo JSON dentro da pasta "Data".
        /// </summary>
        /// <param name="fileName">Nome do arquivo (sem extensão).</param>
        /// returns>Caminho completo do arquivo JSON.</returns>
        private static string GetFilePath(string fileName) => $"Data/{fileName}.json";


        /// <summary>
        /// Carrega a lista de objetos do arquivo JSON correspondente.
        /// Se o arquivo não existir, retorna uma lista vazia.
        /// </summary>
        /// <param name="fileName">Nome do arquivo (sem extensão).</param>
        /// returns>Lista de objetos do tipo T</returns>
        public static List<T> Load(string fileName)
        {
            var path = GetFilePath(fileName);

            // Se o arquivo não existir, retorna uma lista vazia.
            if (!File.Exists(path)) return new List<T>();

            // Lê o conteúdo do arquivo JSON.
            var json = File.ReadAllText(path);

            // Converte o JSON para uma lista de objetos do tipo T.
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

        }


        /// <summary>
        /// Salva uma lista de objetos em um arquivo JSON correspondente.
        /// Cria a pasta "Data" se não existir.
        /// </summary>
        /// <param name="fileName">Nome do arquivo (sem extensão).</param>
        /// <param name="data">Lista de objetos a ser salva</param>

        public static void Save(string fileName, List<T> data)
        {
            var path = GetFilePath(fileName);
            var directory = Path.GetDirectoryName(path);

            // Cria o diretório "Data" se não existir.
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Serializa a lista para JSON formatado.
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            // Escreve o JSON no arquivo.
            File.WriteAllText(path, json);
        }
    }
}
