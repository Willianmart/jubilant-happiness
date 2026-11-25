
namespace GerenciadorDeTarefas
{
    // Classe responsável por guardar os dados de cada tarefa.
    class TaskItem
    {
        public int Id { get; }
        public string Descricao { get; }
        public bool Concluida { get; set; }

        // Construtor que recebe o ID e a descrição.
        public TaskItem(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
            Concluida = false; // Toda tarefa começa como pendente.
        }
    }
}
