using EclipseWorks.Domain.Models;

namespace EclipseWorks.Domain.Repositories.Base;
public interface IRepository
{

    #region Projeto
    List<Projeto> ListarProjetos(string IdUsuario);
    void AdicionarProjeto(Projeto projeto);
    void RemoverProjeto(int IdProjeto);
    #endregion Projeto


    #region Tarefa
    void AdicionarTarefa(int IdProjeto, Tarefa tarefa);
    void AtualizarTarefa(int IdTarefa, string IdUsuario, Tarefa tarefaUpd);
    void RemoverTarefa(int Id);
    void AdicionarComentarioTarefa(Comentario comentario);
    #endregion Tarefa

    #region Relatorio
    decimal CalcularMediaTarefasConcluidas(string idUsuario);
    #endregion Relatorio
}