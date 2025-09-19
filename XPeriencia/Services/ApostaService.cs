using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPeriencia.Models;

namespace XPeriencia.Services
{
    public static class ApostaService
    {
        private static readonly string FileName = "apostas";
        private static readonly string UsuarioFile = "usuarios";

        public static void Menu()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("=== Gerenciar Apostas ===");
                Console.WriteLine("1. Registrar Aposta");
                Console.WriteLine("2. Listar Apostas");
                Console.WriteLine("3. Remover Aposta");
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
            var apostas = DataManager<Aposta>.Load(FileName);
            var usuarios = DataManager<Usuario>.Load(UsuarioFile);

            if(usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado. Cadastre um usuário primeiro.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("===== Usuários Disponíveis =====");
            foreach(var u in usuarios) 
                Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome} | Pontos: {u.Pontos}");

            Console.Write("Digite o ID do usuário que vai apostar: ");
            if (!int.TryParse(Console.ReadLine(), out int usuarioId)) return;

            var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadKey();
                return;
            }

            Console.Write("Qual será a Aposta: ");
            var descricao = Console.ReadLine();

            Console.Write("Valor da aposta (em pontos): ");
            if (!int.TryParse(Console.ReadLine(), out int valor)) return;

            Console.Write("Resultado (G = ganha | P = perde): ");
            var resultado = Console.ReadLine()?.ToUpper();

            bool ganhou = resultado == "G";

            // Atualizar pontos do usuário
            if (ganhou)
                usuario.Pontos += valor;
            else
                usuario.Pontos -= valor;

            // Criar aposta
            var novaAposta = new Aposta
            {
                Id = apostas.Count > 0 ? apostas.Max(a => a.Id) + 1 : 1,
                UsuarioId = usuario.Id,
                Descricao = descricao ?? "Sem Descrição",
                Valor = valor,
                Ganhou = ganhou
            };

            novaAposta.Data = DateTime.Now;

            apostas.Add(novaAposta);

            //Salvar alterações
            DataManager<Aposta>.Save(FileName, apostas);
            DataManager<Usuario>.Save(UsuarioFile, usuarios);

            Console.WriteLine("Aposta registrada com sucesso!");
            Console.ReadKey();
        }

        private static void Listar()
        {
            var apostas = DataManager<Aposta>.Load(FileName);
            var usuarios = DataManager<Usuario>.Load(UsuarioFile);

            Console.WriteLine("===== Lista de Apostas =====");
            foreach(var a in apostas)
            {
                var usuario = usuarios.FirstOrDefault(u => u.Id == a.UsuarioId);
                var nomeUsuario = usuario != null ? usuario.Nome : "Desconhecido";
                var resultado = a.Ganhou ? "Ganhou" : "Perdeu";

                Console.WriteLine($"ID: {a.Id} | Usuário: {nomeUsuario} | Descrição: {a.Descricao} | Valor: {a.Valor} | Data: {a.Data} | Resultado: {resultado}");
            }

            if(apostas.Count == 0)
                Console.WriteLine("Nenhuma aposta registrada.");

            Console.ReadKey();
        }

        private static void Remover()
        {
            var apostas = DataManager<Aposta>.Load(FileName);

            Console.Write("Digite o ID da aposta que deseja remover: ");
            if(!int.TryParse(Console.ReadLine(), out int apostaId)) return;

            var aposta = apostas.FirstOrDefault(a => a.Id == apostaId);
            if(aposta == null)
            {
                Console.WriteLine("Aposta não encontrada.");
                Console.ReadKey();
                return;
            }

            apostas.Remove(aposta);
            DataManager<Aposta>.Save(FileName, apostas);

            Console.WriteLine("Aposta removida com sucesso!");
            Console.ReadKey();
        }
    }
}
