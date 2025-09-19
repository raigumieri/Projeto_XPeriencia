using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPeriencia.Models;

namespace XPeriencia.Services
{
    public static class ReflexaoService
    {
        private static readonly string fileName = "reflexoes";
        private static readonly string usuarioFile = "usuarios";

        public static void Menu()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("===== Registro de Reflexões =====");
                Console.WriteLine("1. Registrar Reflexão");
                Console.WriteLine("2. Listar Reflexões");
                Console.WriteLine("3. Remover Reflexão");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");
                Console.WriteLine();

                if (!int.TryParse(Console.ReadLine(), out opcao)) opcao = -1;

                switch (opcao)
                {
                    case 1:
                        Registrar();
                        break;
                    case 2:
                        Listar();
                        break;
                    case 3:
                        Remover();
                        break;
                }
            } while (opcao != 0);
        }

        private static void Registrar()
        {
            var reflexoes = DataManager<Reflexao>.Load(fileName); 
            var usuarios = DataManager<Usuario>.Load(usuarioFile);

            if (usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado. Cadastre um usuário primeiro.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("===== Usuários Disponíveis =====");
            foreach (var u in usuarios) 
                Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome}");

            Console.Write("Digite o ID do usuário que deseja registrar a reflexão: ");
            if (!int.TryParse(Console.ReadLine(), out int usuarioId)) return;

            var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if(usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadKey();
                return;
            }

            Console.Write("Digite como você está se sentindo: ");
            var sentimento = Console.ReadLine();

            var novaReflexao = new Reflexao
            {
                Id = reflexoes.Count == 0 ? 1 : reflexoes.Max(r => r.Id) + 1,
                UsuarioId = usuario.Id,
                Sentimento = string.IsNullOrWhiteSpace(sentimento) ? "Sem Descrição" : sentimento
            };

            reflexoes.Add(novaReflexao);
            DataManager<Reflexao>.Save(fileName, reflexoes);

            Console.WriteLine("Reflexão registrada com sucesso!");
            Console.ReadKey();
        }

        private static void Listar()
        {
            var reflexoes = DataManager<Reflexao>.Load(fileName);
            var usuarios = DataManager<Usuario>.Load(usuarioFile);

            Console.WriteLine("===== Lista de Reflexões =====");
            foreach (var r in reflexoes)
            {
                var usuario = usuarios.FirstOrDefault(u => u.Id == r.UsuarioId);
                var nomeUsuario = usuario != null ? usuario.Nome : "Usuário Desconhecido";  

                Console.WriteLine($"ID: {r.Id} | Usuário: {nomeUsuario} | Data: {r.Data} |Sentimento: {r.Sentimento}");
            }

            if (reflexoes.Count == 0)
                Console.WriteLine("Nenhuma reflexão registrada.");

            Console.ReadKey();
        }

        private static void Remover()
        {
            var reflexoes = DataManager<Reflexao>.Load(fileName);

            Console.Write("ID da Reflexão que deseja remover: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var reflexao = reflexoes.FirstOrDefault(r => r.Id == id);
            if (reflexao == null)
            {
                Console.WriteLine("Reflexão não encontrada.");
                Console.ReadKey();
                return;
            }

            reflexoes.Remove(reflexao);
            DataManager<Reflexao>.Save(fileName, reflexoes);

            Console.WriteLine("Reflexão removida com sucesso!");
            Console.ReadKey();
        }
    }
}
