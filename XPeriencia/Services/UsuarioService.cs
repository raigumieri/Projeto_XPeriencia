using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPeriencia.Models;

namespace XPeriencia.Services
{
    public static class UsuarioService
    {
        private static readonly string FileName = "usuarios";

        public static void Menu()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("=== Gerenciamento de Usuários ===");
                Console.WriteLine("1. Cadastrar Usuário");
                Console.WriteLine("2. Listar Usuários");
                Console.WriteLine("3. Atualizar Usuário");
                Console.WriteLine("4. Deletar Usuário");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");
                Console.WriteLine();

                if (!int.TryParse(Console.ReadLine(), out opcao)) opcao = -1;

                switch (opcao)
                {
                    case 1:
                        Cadastrar();
                        break;

                    case 2:
                        Listar();
                        break;

                    case 3:
                        Atualizar();
                        break;

                    case 4:
                        Remover();
                        break;
                }

            } while (opcao != 0);

        }

        private static void Cadastrar()
        {
            var usuarios = DataManager<Usuario>.Load(FileName);

            /*
            Console.Write("Nome do Usuário: ");
            var nome = Console.ReadLine();
            */

            var usuario = new Usuario
            {
                Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1,
                Pontos = 0
            };

            Console.Write("Nome do Usuário: ");
            usuario.Nome = Console.ReadLine() ?? "";

            Console.Write("Email do Usuário: ");
            usuario.Email = Console.ReadLine() ?? "";

            usuario.DataCriacao = DateTime.Now;

            usuarios.Add(usuario);
            DataManager<Usuario>.Save(FileName, usuarios);

            Console.WriteLine("Usuário cadastrado com sucesso!");
            Console.ReadKey();
        }

        private static void Listar()
        {
            var usuarios = DataManager<Usuario>.Load(FileName);

            Console.WriteLine("=== Lista de Usuários ===");
            foreach(var u in usuarios)
            {
                Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome} | Email: {u.Email} | Pontos: {u.Pontos} | Data de Criação: {u.DataCriacao}");
            }


            if (usuarios.Count == 0)
                Console.WriteLine("Nenhum usuário cadastrado.");

            Console.ReadKey();

        }

        private static void Atualizar()
        {
            var usuarios = DataManager<Usuario>.Load(FileName);

            Console.Write("Digite o ID do usuário que deseja atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if(usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Novo nome ({usuario.Nome}): ");
            var nome = Console.ReadLine();

            if(!string.IsNullOrWhiteSpace(nome))
                usuario.Nome = nome;

            DataManager<Usuario>.Save(FileName, usuarios);

            Console.WriteLine("Usuário atualizado com sucesso!");
            Console.ReadKey();

        }

        private static void Remover()
        {
            var usuarios = DataManager<Usuario>.Load(FileName);

            Console.Write("Digite o ID do usuário que deseja deletar: ");
            if(!int.TryParse(Console.ReadLine(), out int id)) return;

            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadKey();
                return;
            }

            usuarios.Remove(usuario);
            DataManager<Usuario>.Save(FileName, usuarios);

            Console.WriteLine("Usuário deletado com sucesso!");
            Console.ReadKey();
        }
       
    }
}
