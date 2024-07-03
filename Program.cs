using System;
using System.IO;

class Program
{
    private static string[] produtos = new string[4];
    private static int[] estoque = new int[4];
    private static int[,] vendas = new int[4, 31];

    private const int DiasNoMes = 31;

    static void Main()
    {
        int opcao;

        do
        {
            Console.WriteLine("Menu Principal:");
            Console.WriteLine("1 - Importar arquivo de produtos");
            Console.WriteLine("2 - Registrar venda");
            Console.WriteLine("3 - Relatório de vendas");
            Console.WriteLine("4 - Relatório de estoque");
            Console.WriteLine("5 - Criar arquivo de vendas");
            Console.WriteLine("6 - Sair");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida, tente novamente!");
                continue;
            }

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

    static void ImportarArquivoProdutos()
    {
        try
        {
            string[] linhas = File.ReadAllLines("produtos.txt");

            produtos = new string[linhas.Length];
            estoque = new int[linhas.Length];

            for (int i = 0; i < linhas.Length; i++)
            {
                string[] dados = linhas[i].Split(' ');
                if (dados.Length >= 2)
                {
                    produtos[i] = dados[0];
                    estoque[i] = int.Parse(dados[1]);
                }
                else
                {
                    Console.WriteLine($"Erro ao ler dados da linha {i}: Formato inválido.");
                }
            }

            vendas = new int[produtos.Length, DiasNoMes];

            Console.WriteLine("Importação do Arquivo de produtos obteve êxito!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Arquivo de produtos não encontrado! Lembre-se de criar o arquivo 'produtos.txt' antes de importá-lo.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao importar arquivo de produtos! Detalhes: {ex.Message}");
        }
    }

    static void RegistrarVenda()
    {
        if (produtos == null || produtos.Length == 0)
        {
            Console.WriteLine("Importe o arquivo de produtos e depois registre as vendas.");
            return;
        }

        Console.WriteLine("Digite o código do produto (1 a 4):");

        if (!int.TryParse(Console.ReadLine(), out int produto) || produto < 1 || produto > produtos.Length)
        {
            Console.WriteLine("Número de produto inválido.");
            return;
        }

        Console.WriteLine("Digite o dia do mês (1 a 31):");

        if (!int.TryParse(Console.ReadLine(), out int dia) || dia < 1 || dia > DiasNoMes)
        {
            Console.WriteLine("Dia do mês inválido.");
            return;
        }

        Console.WriteLine("Digite a quantidade vendida:");

        if (!int.TryParse(Console.ReadLine(), out int quantidade))
        {
            Console.WriteLine("Quantidade inválida!");
            return;
        }

        if (quantidade > estoque[produto - 1])
        {
            Console.WriteLine("Cuidado! Quantidade vendida não pode ultrapassar o estoque.");
            return;
        }

        vendas[produto - 1, dia - 1] += quantidade;
        estoque[produto - 1] -= quantidade;

        // Atualizar arquivo de produtos com novo estoque
        AtualizarArquivoProdutos();

        Console.WriteLine("Venda registrada com sucesso.");
    }

    static void AtualizarArquivoProdutos()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("produtos.txt"))
            {
                for (int i = 0; i < produtos.Length; i++)
                {
                    writer.WriteLine($"{produtos[i]} {estoque[i]}");
                }
            }
            Console.WriteLine("Estoque atualizado no arquivo 'produtos.txt'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar o arquivo de produtos: {ex.Message}");
        }
    }

    static void RelatorioVendas()
    {
        if (produtos == null || produtos.Length == 0 || vendas == null)
        {
            Console.WriteLine("Importe o arquivo de produtos, registre as vendas e depois gere o relatório.");
            return;
        }

        Console.WriteLine("Relatório de Vendas:");

        Console.Write("\t");
        for (int i = 0; i < produtos.Length; i++)
        {
            Console.Write($"{produtos[i]}\t");
        }
        Console.WriteLine();

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

    static void RelatorioEstoque()
    {
        if (produtos == null || produtos.Length == 0)
        {
            Console.WriteLine("Importe o arquivo de produtos e depois solicite o relatório de estoque.");
            return;
        }

        Console.WriteLine("Relatório de Estoque:");

        Console.WriteLine("Produto\tEstoque Atual");
        for (int i = 0; i < produtos.Length; i++)
        {
            Console.WriteLine($"{produtos[i]}\t{estoque[i]}");
        }
    }

    static void CriarArquivoVendas()
    {
        if (produtos == null || produtos.Length == 0 || vendas == null)
        {
            Console.WriteLine("Importe o arquivo de produtos, registre as vendas e depois crie o arquivo de vendas.");
            return;
        }

        try
        {
            using (StreamWriter writer = new StreamWriter("total_vendas.txt"))
            {
                for (int i = 0; i < produtos.Length; i++)
                {
                    writer.Write($"{produtos[i]}\t");
                }
                writer.WriteLine();

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
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar o arquivo de vendas! Detalhes: {ex.Message}");
        }
    }
}
