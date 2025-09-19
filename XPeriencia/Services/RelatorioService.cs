using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPeriencia.Models;

namespace XPeriencia.Services
{
    public static class RelatorioService
    {
        private static readonly string UsuarioFile = "usuarios";
        private static readonly string ApostaFile = "apostas";
        private static readonly string ReflexaoFile = "reflexoes";
        private static readonly string PastaRelatorios = "Relatorios";

        public static void Menu()
        {
            int opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("===== Relatórios =====");
                Console.WriteLine("1. Gerar Relatório de Usuário");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");
                Console.WriteLine();

                if (!int.TryParse(Console.ReadLine(), out opcao)) opcao = -1;

                switch (opcao)
                {
                    case 1:
                        GerarRelatorioUsuario();
                        break;
                }
            }while (opcao != 0);
        }

        private static void GerarRelatorioUsuario()
        {
            var usuarios = DataManager<Usuario>.Load(UsuarioFile);
            var apostas = DataManager<Aposta>.Load(ApostaFile);
            var reflexoes = DataManager<Reflexao>.Load(ReflexaoFile);

            if (usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado. Cadastre um usuário primeiro.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("===== Usuários Disponíveis =====");
            foreach (var u in usuarios)
                Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome}");

            Console.Write("Digite o ID do usuário para gerar o relatório: ");
            if (!int.TryParse(Console.ReadLine(), out int usuarioId)) return;

            var usuario = usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadKey();
                return;
            }

            //Monta o conteúdo do relatório
            var relatorio = new List<string>
            {
                $"===== RELATÓRIO DO USUÁRIO =====",
                $"Nome: {usuario.Nome}",
                $"Email: {usuario.Email}",
                $"Data de Criação: {usuario.DataCriacao}",
                "",

                "===== APOSTAS ===== "
            };

            var apostasUsuario = apostas.Where(a => a.UsuarioId == usuario.Id).ToList();
            if(apostasUsuario.Count > 0)
            {
                foreach(var a in apostasUsuario)
                {
                    relatorio.Add($"ID: {a.Id} | Data: {a.Data} | Aposta: {a.Descricao} | Resultado: {a.Ganhou}");
                }

            }
            else
            {
                relatorio.Add("Nenhuma aposta registrada.");
            }

            relatorio.Add("");
            relatorio.Add("===== REFLEXÕES ===== ");

            var reflexoesUsuario = reflexoes.Where(r => r.UsuarioId == usuario.Id).ToList();
            if(reflexoesUsuario.Count > 0)
            {
                foreach (var r in reflexoesUsuario)
                {
                    relatorio.Add($"ID: {r.Id} | Data: {r.Data} | Sentimento: {r.Sentimento}");
                }
            }
            else
            {
                relatorio.Add("Nenhuma reflexão registrada.");
            }

            // ===== ESTATÍSTICAS =====
            relatorio.Add("");
            relatorio.Add("===== ESTATÍSTICAS =====");

            relatorio.Add($"Total de Apostas: {apostasUsuario.Count}");
            relatorio.Add($"Total de Reflexões: {reflexoesUsuario.Count}");

            if(apostasUsuario.Count > 0)
            {
                var somaValores = apostasUsuario.Sum(a => a.Valor);
                var maiorAposta = apostasUsuario.Max(a => a.Valor);
                var menorAposta = apostasUsuario.Min(a => a.Valor);
                var mediaApostas = apostasUsuario.Average(a => a.Valor);

                relatorio.Add($"Soma dos Valores Apostados: {somaValores}");
                relatorio.Add($"Maior valor em aposta: {maiorAposta}");
                relatorio.Add($"Menor valor em aposta: {menorAposta}");
                relatorio.Add($"Média dos valores apostados: {mediaApostas:F2}");


            }
            else
            {
                relatorio.Add("Nenhuma estatística disponível para apostas.");
            }

            //Salvar o relatório em um arquivo
            if (!Directory.Exists(PastaRelatorios))
                Directory.CreateDirectory(PastaRelatorios);

            var nomeArquivo = Path.Combine(PastaRelatorios, $"Relatorio_Usuario_{usuario.Nome}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllLines(nomeArquivo, relatorio);

            Console.WriteLine($"Relatório gerado com sucesso: {nomeArquivo}");
            Console.ReadKey();

        }

    }
}
