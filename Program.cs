using System;
using System.IO;

//TRABALHO FINAL ATP - GERENCIANDO CONTROLE DE VENDAS DE 04 PRODUTOS

//Criando class Program para armazenar todo o programa
class Program
{
    private static string[] produtos = new string [4]; // Declarando vetor que armazena o nome dos produtos
    private static int[] estoque = new int [4]; // Declarando vetor que armazena o estoque de cada produto
    private static int[,] vendas = new int [4,31]; // Declarando matriz para armazenar a quantidade de vendas diárias de cada produto

//Definindo valor constante para a quantidade de dias em 1 mês
    private const int DiasNoMes = 31;

//Criando método Main como ponto de entrada para o menu principal
    static void Main()
    {
        int opcao;

// Criando método do-while para gerar as opções do usuário antes do 6 "sair" ser escolhido
        do
        {
            Console.WriteLine("Menu Principal:");
            Console.WriteLine("1 - Importar arquivo de produtos");
            Console.WriteLine("2 - Registrar venda");
            Console.WriteLine("3 - Relatório de vendas");
            Console.WriteLine("4 - Relatório de estoque");
            Console.WriteLine("5 - Criar arquivo de vendas");
            Console.WriteLine("6 - Sair");

// Criando comando if condicional para validar a opção do usuário
            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida, tente novamente!");
                continue;
            }
// Criando comando switch case para gerar a opção correspondente à escolha do usuário
            switch (opcao)
            {
                case 1:
                    ImportarArquivoProdutos();
                    break;
                case 2:
                    RegistrarVenda();
                    break;
                case 3:
                    RelatorioVendas();
                    break;
                case 4:
                    RelatorioEstoque();
                    break;
                case 5:
                    CriarArquivoVendas();
                    break;
                case 6:
                    Console.WriteLine("Programa encerrado.");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Digite um número entre 1 e 6.");
                    break;
            }

        } while (opcao != 6);
    }

// Criando procedimento ImportarArquivoProdutos para fazer a importação do arquivo que vai servir de base para execução do programa
    static void ImportarArquivoProdutos()
    {
        try
        {
// Criando comando para fazer a leitura do arquivo de produtos e suas quantidades em estoque
            string[] linhas = File.ReadAllLines("produtos.txt");

// Declarando tamanho dos vetores, de acordo com a quantidade de produtos 
            produtos = new string[linhas.Length];
            estoque = new int[linhas.Length];

// Preenchendo vetores, de acordo com a quantidade de produtos
            for (int i = 0; i < linhas.Length; i++)
            {
                string[] dados = linhas[i].Split('\t');
                produtos[i] = dados[0];
                estoque[i] = int.Parse(dados[1]);
            }

// Declarando matriz de vendas com quantidade de produtos e 31 dias, e avisando usuário
            vendas = new int[produtos.Length, DiasNoMes];

            Console.WriteLine("Importação do Arquivo de produtos obteve êxito!");
        }

// Alertando o usuário de possíveis problemas com o arquivo

        catch (FileNotFoundException)
        {
            Console.WriteLine("Arquivo de produtos não encontrado! Lembre-se de criar o arquivo 'produtos.txt' antes de importá-lo.");
        }
        catch (Exception)
        {
            Console.WriteLine($"Erro ao importar arquivo de produtos! Verifique e tente novamente.");
        }
    }

// Criando procedimento para registrar as vendas informadas pelo usuário
    static void RegistrarVenda()
    {
        if (produtos == null || produtos.Length == 0)
        {
            Console.WriteLine("Importe o arquivo de produtos e depois registre as vendas.");
            return;
        }

        Console.WriteLine("Digite o código do produto (A, B, C ou D):");

// Criando comando if condicional para garantir que o usuário informou um código de produto válido

        if (!int.TryParse(Console.ReadLine(), out int produto) || produto < 1 || produto > produtos.Length)
        {
            Console.WriteLine("Número de produto inválido.");
            return;
        }

        Console.WriteLine("Digite o dia do mês (1 a 31):");

// Criando comando if condicional para garantir que o usuário informou um dia do mês válido

        if (!int.TryParse(Console.ReadLine(), out int dia) || dia < 1 || dia > DiasNoMes)
        {
            Console.WriteLine("Dia do mês inválido.");
            return;
        }

        Console.WriteLine("Digite a quantidade vendida:");

// Criando comando if condicional para garantir que o usuário informou uma quantidade de produto válida

        if (!int.TryParse(Console.ReadLine(), out int quantidade))
        {
            Console.WriteLine("Quantidade inválida!");
            return;
        }

// Criando comando if condicional para garantir que o número de vendas não ultrapasse o número em estoque

        if (quantidade > estoque[produto - 1])
        {
            Console.WriteLine("Cuidado! Quantidade vendida não pode ultrapassar o estoque.");
            return;
        }

// Criando cálculos para atualizar a quantidade em estoque

        vendas[produto - 1, dia - 1] += quantidade;
        estoque[produto - 1] -= quantidade;

        Console.WriteLine("Venda registrada com sucesso.");
    }

// Criando procedimento RelatorioVendas para gerar e imprimir relatório de vendas
    static void RelatorioVendas()
    {
        if (produtos == null || vendas == null)
        {
            Console.WriteLine("Importe o arquivo de produtos, registre as vendas e depois gere o relatório.");
            return;
        }

        Console.WriteLine("Relatório de Vendas:");

// Comandos para fazer um cabeçalho com o nome dos produtos

        Console.Write("\t");
        for (int i = 0; i < produtos.Length; i++)
        {
            Console.Write($"{produtos[i]}\t");
        }
        Console.WriteLine();

// Comandos para gerar e imprimir a matriz

        for (int dia = 0; dia < DiasNoMes; dia++)
        {
            Console.Write($"Dia {dia + 1}\t");
            for (int produto = 0; produto < produtos.Length; produto++)
            {
                Console.Write($"{vendas[produto, dia]}\t");
            }
            Console.WriteLine();
        }
    }

// Criando procedimento RelatorioEstoque para gerar e imprimir relatório de estoque
    static void RelatorioEstoque()
    {
        if (produtos == null)
        {
            Console.WriteLine("Importe o arquivo de produtos e depois solicite o relatório de estoque.");
            return;
        }
        Console.WriteLine("Relatório de Estoque:");

// Comandos para fazer um cabeçalho com o nome dos produtos e estoques atualizados

        Console.Write("Produto\tEstoque Atual\n");

// Imprimindo números do vetor estoque
        
        for (int i = 0; i < produtos.Length; i++)
        {
            Console.WriteLine($"{produtos[i]}\t{estoque[i]}");
        }
    }

// Criando procedimento para criar o arquivo de vendas
    static void CriarArquivoVendas()
    {
        if (produtos == null || vendas == null)
        {
            Console.WriteLine("Importe o arquivo de produtos, registre as vendas e depois crie o arquivo de vendas.");
            return;
        }

        try
        {

// Criando o arquivo "total_vendas.txt"

            using (StreamWriter writer = new StreamWriter("total_vendas.txt"))
            {

// Criando um cabeçalho no arquivo com o nome dos produtos

                for (int i = 0; i < produtos.Length; i++)
                {
                    writer.Write($"{produtos[i]}\t");
                }
                writer.WriteLine();

//Completando o arquivo total_vendas com os dados de vendas

                for (int dia = 0; dia < DiasNoMes; dia++)
                {
                    writer.Write($"Dia {dia + 1}\t");
                    for (int produto = 0; produto < produtos.Length; produto++)
                    {
                        writer.Write($"{vendas[produto, dia]}\t");
                    }
                    writer.WriteLine();
                }
            }
            Console.WriteLine("A criação do arquivo de total de vendas obteve êxito!");
        }
        catch (Exception)
        {
            Console.WriteLine($"Erro ao criar o arquivo de vendas! Verifique e tente novamente.");
        }
    }
}
