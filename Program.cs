using System;
using System.Collections.Generic;

namespace GerenciadorDeTarefas
{
    // Criei essa classe para guardar os dados de cada tarefa.
    class Tarefa
    {
        public int Id { get; }
        public string Descricao { get; }
        public bool Concluida { get; set; }

        // No construtor, recebo o ID e a descrição da tarefa.
        public Tarefa(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
            Concluida = false; // Toda tarefa começa como pendente.
        }
    }

    class Program
    {
        // Aqui guardo todas as tarefas que aparecem no gerenciador.
        static readonly List<Tarefa> tarefas = new List<Tarefa>();

        // Uso a pilha para lembrar qual tarefa concluí por último.
        static readonly Stack<int> pilhaConclusoes = new Stack<int>();

        // Começo os IDs em 1 e avanço o contador a cada cadastro.
        static int proximoId = 1;

        static void Main()
        {
            // Carrego os exemplos antes do menu para eles aparecerem só uma vez por execução.
            CarregarTarefasIniciais();

            int opcao;

            // Repito o menu até o usuário escolher a opção de sair.
            do
            {
                Console.Clear();
                Console.WriteLine("=== GERENCIADOR DE TAREFAS ===");
                Console.WriteLine("1 - Cadastrar tarefa");
                Console.WriteLine("2 - Listar tarefas");
                Console.WriteLine("3 - Buscar tarefa");
                Console.WriteLine("4 - Concluir tarefa");
                Console.WriteLine("5 - Desfazer última conclusão");
                Console.WriteLine("6 - Sair");
                Console.Write("\nEscolha uma opção: ");

                // Verifico se o usuário digitou um número inteiro.
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número de 1 a 6.");
                }
                else
                {
                    Console.Clear();

                    switch (opcao)
                    {
                        case 1:
                            CadastrarTarefa();
                            break;

                        case 2:
                            ListarTarefas();
                            break;

                        case 3:
                            BuscarTarefa();
                            break;

                        case 4:
                            ConcluirTarefa();
                            break;

                        case 5:
                            DesfazerConclusao();
                            break;

                        case 6:
                            Console.WriteLine("Sistema encerrado. Até logo!");
                            break;

                        default:
                            Console.WriteLine("Opção inválida! Escolha de 1 a 6.");
                            break;
                    }
                }

                if (opcao != 6)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                }
            }
            while (opcao != 6);
        }

        static void CarregarTarefasIniciais()
        {
            // Deixei essas tarefas prontas para poder testar o programa assim que ele abrir.
            AdicionarTarefa("Revisar a aula de C# sobre listas e pilhas");
            AdicionarTarefa("Terminar a lista de exercícios de programação");
            AdicionarTarefa("Testar a busca de uma tarefa pelo ID");
        }

        static Tarefa AdicionarTarefa(string descricao)
        {
            // Uso o mesmo contador para as tarefas iniciais e para as cadastradas pelo menu.
            Tarefa novaTarefa = new Tarefa(proximoId, descricao);
            tarefas.Add(novaTarefa);
            proximoId++;

            return novaTarefa;
        }

        // 1 - Cadastrar uma nova tarefa.
        static void CadastrarTarefa()
        {
            Console.WriteLine("=== CADASTRAR TAREFA ===");
            Console.Write("Digite a descrição da tarefa: ");

            string descricao = (Console.ReadLine() ?? "").Trim();

            // Se o usuário só apertar Enter ou digitar espaços, peço uma descrição.
            if (descricao.Length == 0)
            {
                Console.WriteLine("Informe uma descrição para cadastrar a tarefa.");
                return;
            }

            // Guardo a tarefa que acabei de cadastrar para mostrar o ID que ela recebeu.
            Tarefa novaTarefa = AdicionarTarefa(descricao);

            Console.WriteLine("Tarefa cadastrada com sucesso!");
            ExibirTarefa(novaTarefa);
        }

        // 2 - Listar todas as tarefas cadastradas.
        static void ListarTarefas()
        {
            Console.WriteLine("=== LISTA DE TAREFAS ===");

            // Antes de percorrer a lista, verifico se existe alguma tarefa.
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
                return;
            }

            // Percorro a lista e mostro os dados de cada tarefa.
            foreach (Tarefa tarefa in tarefas)
            {
                ExibirTarefa(tarefa);
            }
        }

        // 3 - Buscar uma tarefa pelo ID.
        static void BuscarTarefa()
        {
            Console.WriteLine("=== BUSCAR TAREFA ===");
            Console.Write("Digite o ID da tarefa que deseja buscar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido! Digite um número inteiro.");
                return;
            }

            // Procuro na lista a tarefa que tem o mesmo ID que o usuário digitou.
            var tarefa = tarefas.Find(t => t.Id == id);

            if (tarefa == null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o ID {id}.");
                return;
            }

            ExibirTarefa(tarefa);
        }

        // 4 - Concluir uma tarefa pelo ID.
        static void ConcluirTarefa()
        {
            Console.WriteLine("=== CONCLUIR TAREFA ===");
            Console.Write("Digite o ID da tarefa que deseja concluir: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido! Digite um número inteiro.");
                return;
            }

            // Busco a tarefa antes de alterar a situação dela.
            var tarefa = tarefas.Find(t => t.Id == id);

            if (tarefa == null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o ID {id}.");
                return;
            }

            // Evito registrar a mesma conclusão duas vezes na pilha.
            if (tarefa.Concluida)
            {
                Console.WriteLine($"A tarefa {id} já está concluída.");
                return;
            }

            // Marco como concluída e guardo o ID no topo da pilha para poder desfazer depois.
            tarefa.Concluida = true;
            pilhaConclusoes.Push(id);

            Console.WriteLine($"Tarefa {id} concluída com sucesso!");
        }

        // 5 - Desfazer a última conclusão.
        static void DesfazerConclusao()
        {
            Console.WriteLine("=== DESFAZER ÚLTIMA CONCLUSÃO ===");

            // Se a pilha estiver vazia, não tenho nenhuma conclusão para desfazer.
            if (pilhaConclusoes.Count == 0)
            {
                Console.WriteLine("Não há conclusões para desfazer.");
                return;
            }

            // Com o Pop, retiro o último ID que coloquei na pilha.
            int idUltimaConclusao = pilhaConclusoes.Pop();
            var tarefa = tarefas.Find(t => t.Id == idUltimaConclusao);

            if (tarefa != null)
            {
                // Volto a situação dessa tarefa para pendente.
                tarefa.Concluida = false;
                Console.WriteLine($"A tarefa {idUltimaConclusao} voltou para pendente.");
            }
        }

        static void ExibirTarefa(Tarefa tarefa)
        {
            // Uso esse método no cadastro, na listagem e na busca para mostrar os mesmos dados.
            string situacao = tarefa.Concluida ? "Concluída" : "Pendente";

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"ID: {tarefa.Id}");
            Console.WriteLine($"Descrição: {tarefa.Descricao}");
            Console.WriteLine($"Situação: {situacao}");
        }
    }
}