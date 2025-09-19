using XPeriencia.Models;
using XPeriencia.Services;

/// <summary>
/// Classe principal do programa XPeriência.
/// Responsavel por inicializar o programa e exibir o menu principal
/// para navegação entre os serviços de usuários, apostas, reflexões e relatórios.
/// </summary>
class Program
{
    /// <summary>
    /// Ponto de entrada do programa.
    /// Exibe o menu principal e redireciona o usuário para o serviço correspondente.
    /// </summary>
    static void Main(string[] args)
    {
        int opcao; // Variável para armazenar a opção escolhida pelo usuário
        do
        {
            // Limpa a tela e exibe o menu principal
            Console.Clear();
            Console.WriteLine("===== XPeriência =====");
            Console.WriteLine("1. Gerenciar Usuários");
            Console.WriteLine("2. Registrar Apostas (fictícia)");
            Console.WriteLine("3. Registrar Reflexões");
            Console.WriteLine("4. Relatórios");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            // Lê a opção do usuário e valida se é um número inteiro
            if (!int.TryParse(Console.ReadLine(), out opcao)) opcao = -1;

            // Redireciona para o serviço correspondente com base na opção escolhida
            switch (opcao)
            {
                case 1:
                    // Chama o menu de gerenciamento de usuários
                    UsuarioService.Menu();
                    break;

                case 2:
                    // Chama o menu de registro de apostas
                    ApostaService.Menu();
                    break;

                case 3:
                    // Chama o menu de registro de reflexões
                    ReflexaoService.Menu();
                    break;

                case 4:
                    // Chama o menu de relatórios
                    RelatorioService.Menu();
                    break;
            }

        }while (opcao != 0); // Continua exibindo o menu até o usuário escolher sair
    }
}
