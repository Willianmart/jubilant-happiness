
using System;
using System.Collections.Generic;

namespace GerenciadorDeTarefas
{
    class Program
    {
        // Lista que armazena todas as tarefas.
        static readonly List<TaskItem> tarefas = new List<TaskItem>();

        // Pilha que guarda o ID da última tarefa concluída.
        static readonly Stack<int> pilhaConclusoes = new Stack<int>();

        // Contador de IDs.
        static int proximoId = 1;

        static void Main()
        {
            CarregarTarefasIniciais();

            int opcao;

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

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida! Digite um número de 1 a 6.");
                    opcao = 0;
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

            } while (opcao != 6);
        }

        // Carrega tarefas de exemplo.
        static void CarregarTarefasIniciais()
        {
            AdicionarTarefa("Revisar a aula de C# sobre listas e pilhas");
            AdicionarTarefa("Terminar a lista de exercícios de programação");
            AdicionarTarefa("Testar a busca de uma tarefa pelo ID");
        }

        // Adiciona uma tarefa à lista.
        static TaskItem AdicionarTarefa(string descricao)
        {
            TaskItem novaTarefa = new TaskItem(proximoId, descricao);
            tarefas.Add(novaTarefa);
            proximoId++;

            return novaTarefa;
        }

        // 1 - Cadastrar tarefa.
        static void CadastrarTarefa()
        {
            Console.WriteLine("=== CADASTRAR TAREFA ===");
            Console.Write("Digite a descrição da tarefa: ");

            string descricao = (Console.ReadLine() ?? "").Trim();

            if (descricao.Length == 0)
            {
                Console.WriteLine("Informe uma descrição para cadastrar a tarefa.");
                return;
            }

            TaskItem novaTarefa = AdicionarTarefa(descricao);

            Console.WriteLine("Tarefa cadastrada com sucesso!");
            ExibirTarefa(novaTarefa);
        }

        // 2 - Listar tarefas.
        static void ListarTarefas()
        {
            Console.WriteLine("=== LISTA DE TAREFAS ===");

            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
                return;
            }

            foreach (TaskItem tarefa in tarefas)
            {
                ExibirTarefa(tarefa);
            }
        }

        // 3 - Buscar tarefa pelo ID.
        static void BuscarTarefa()
        {
            Console.WriteLine("=== BUSCAR TAREFA ===");
            Console.Write("Digite o ID da tarefa que deseja buscar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido! Digite um número inteiro.");
                return;
            }

            var tarefa = tarefas.Find(t => t.Id == id);

            if (tarefa == null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o ID {id}.");
                return;
            }

            ExibirTarefa(tarefa);
        }

        // 4 - Concluir tarefa.
        static void ConcluirTarefa()
        {
            Console.WriteLine("=== CONCLUIR TAREFA ===");
            Console.Write("Digite o ID da tarefa que deseja concluir: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido! Digite um número inteiro.");
                return;
            }

            var tarefa = tarefas.Find(t => t.Id == id);

            if (tarefa == null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o ID {id}.");
                return;
            }

            if (tarefa.Concluida)
            {
                Console.WriteLine($"A tarefa {id} já está concluída.");
                return;
            }

            tarefa.Concluida = true;
            pilhaConclusoes.Push(id);

            Console.WriteLine($"Tarefa {id} concluída com sucesso!");
        }

        // 5 - Desfazer a última conclusão.
        static void DesfazerConclusao()
        {
            Console.WriteLine("=== DESFAZER ÚLTIMA CONCLUSÃO ===");

            if (pilhaConclusoes.Count == 0)
            {
                Console.WriteLine("Não há conclusões para desfazer.");
                return;
            }

            int idUltimaConclusao = pilhaConclusoes.Pop();
            var tarefa = tarefas.Find(t => t.Id == idUltimaConclusao);

            if (tarefa != null)
            {
                tarefa.Concluida = false;
                Console.WriteLine($"A tarefa {idUltimaConclusao} voltou para pendente.");
            }
        }

        // Exibe os dados de uma tarefa.
        static void ExibirTarefa(TaskItem tarefa)
        {
            string situacao = tarefa.Concluida ? "Concluída" : "Pendente";

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"ID: {tarefa.Id}");
            Console.WriteLine($"Descrição: {tarefa.Descricao}");
            Console.WriteLine($"Situação: {situacao}");
        }
    }
}
