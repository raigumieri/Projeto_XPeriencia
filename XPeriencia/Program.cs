using XPeriencia.Models;
using XPeriencia.Services;

class Program
{
    static void Main(string[] args)
    {
        int opcao;
        do
        {
            Console.Clear();
            Console.WriteLine("===== XPeriência =====");
            Console.WriteLine("1. Gerenciar Usuários");
            Console.WriteLine("2. Registrar Apostas (fictícia)");
            Console.WriteLine("3. Registrar Reflexões");
            Console.WriteLine("4. Relatórios");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            if(!int.TryParse(Console.ReadLine(), out opcao)) opcao = -1;

            switch (opcao)
            {
                case 1:
                    UsuarioService.Menu();
                    break;

                case 2:
                    ApostaService.Menu();
                    break;

                case 3:
                    ReflexaoService.Menu();
                    break;

                case 4:
                    RelatorioService.Menu();
                    break;
            }

        }while (opcao != 0);
    }
}
