using EclipseWorks.Domain.Enums;
using EclipseWorks.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EclipseWorks.Domain.Repositories.Base;
public class Repository : IRepository
{
    private readonly Contexto ctx;
    public Repository(DbContextOptions<Contexto> options)
    {
        ctx = new Contexto(options);
    }

    #region Tarefa
    public void AdicionarTarefa(int IdProjeto, Tarefa tarefa)
    {
        try
        {
            // Verifica se o projeto associado à tarefa está ativo
            var projetoCount = ctx.Projetos.Count(p => p.Id == IdProjeto && p.IsActive);

            if (projetoCount == 0)
            {
                throw new InvalidOperationException("Não é possível adicionar uma tarefa a um projeto inativo.");
            }
            if (projetoCount == 20)
            {
                throw new InvalidOperationException("O Projeto atingiu seu limite máximo de tarefas, sendo ele 20.");
            }

            if (tarefa.Status == Status.Concluida)
            {
                tarefa.DtConclucao = DateTime.Now;
            }

            if (tarefa.Comentarios.Count > 0)
                tarefa.Comentarios.Select(a => { a.IdUsuario = tarefa.IdUsuario; return a; }).ToList();

            tarefa.IdProjeto = IdProjeto;
            ctx.Tarefas.Add(tarefa);
            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void AtualizarTarefa(int IdTarefa, string IdUsuario, Tarefa tarefaUpd)
    {
        try
        {
            var tarefa = ctx.Tarefas.FirstOrDefault(p => p.Id == IdTarefa && p.IsActive == true);

            if (tarefa == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada.");
            }

            //Preenche a data de conclusão da tarefa
            if (tarefa.Status != Status.Concluida && tarefaUpd.Status == Status.Concluida)
            {
                tarefa.DtConclucao = DateTime.Now;
            }

            // Verifica as alterações feitas na tarefa
            List<string> alteracoes = new List<string>();

            if (tarefa.Descricao != tarefaUpd.Descricao)
            {
                alteracoes.Add($"Descrição alterada de '{tarefa.Descricao}' para '{tarefaUpd.Descricao}'");
                tarefa.Descricao = tarefaUpd.Descricao;
            }

            if (tarefa.Status != tarefaUpd.Status)
            {
                alteracoes.Add($"Status alterado de '{tarefa.Status}' para '{tarefaUpd.Status}'");
                tarefa.Status = tarefaUpd.Status;
            }

            // Adiciona as alterações ao histórico
            if (alteracoes.Any())
            {
                var historico = new Historico
                {
                    IdTarefa = tarefa.Id,
                    TpModificacao = "Atualização",
                    Alteracoes = string.Join("; ", alteracoes),
                    DtModificacao = DateTime.Now,
                    UserAlter = IdUsuario
                };

                ctx.Historico.Add(historico);
            }

            // Atualiza a data de alteração e usuário responsável na tarefa
            tarefa.DtAlter = DateTime.Now;
            tarefa.UserAlter = IdUsuario;

            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void RemoverTarefa(int Id)
    {
        try
        {
            var tarefa = ctx.Tarefas.FirstOrDefault(p => p.Id == Id && p.IsActive == true);

            if (tarefa == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrado.");
            }

            tarefa.DtDeleted = DateTime.Now;
            tarefa.IsActive = false;

            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void AdicionarComentarioTarefa(Comentario comentario)
    {
        try
        {
            var tarefa = ctx.Tarefas.FirstOrDefault(p => p.Id == comentario.TarefaId && p.IsActive == true);

            if (tarefa == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada.");
            }

            // Adicionar o comentário ao contexto
            ctx.Comentarios.Add(comentario);
            ctx.SaveChanges(); // Salvando para obter o Id real do comentário

            // Atualizar o histórico com o Id real do comentário
            var historico = new Historico
            {
                IdTarefa = comentario.TarefaId,
                TpModificacao = "Adição de Comentário",
                Alteracoes = "Comentário de ID " + comentario.Id,
                DtModificacao = DateTime.Now,
                UserAlter = comentario.IdUsuario
            };

            ctx.Historico.Add(historico);
            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion Tarefa



    #region Projeto
    public void AdicionarProjeto(Projeto projeto)
    {
        try
        {
            ctx.Projetos.Add(projeto);
            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public List<Projeto> ListarProjetos(string IdUsuario)
    {
        return ctx.Projetos
            .Include(p => p.CriadorDoProjeto) // Inclui os usuários criadores dos projetos
            .Include(p => p.Tarefas) // Inclui as tarefas associadas aos projetos
                .ThenInclude(t => t.Comentarios) // Inclui os comentários associados às tarefas
            .Where(x => x.IdUsuario == IdUsuario && x.IsActive)
            .ToList();
    }
    public void RemoverProjeto(int IdProjeto)
    {
        try
        {
            var projeto = ctx.Projetos.Include(p => p.Tarefas).FirstOrDefault(p => p.Id == IdProjeto && p.IsActive == true);

            if (projeto == null)
            {
                throw new KeyNotFoundException("Projeto não encontrado.");
            }

            if (projeto.Tarefas.Any(t => t.Status == Status.Pendente || t.Status == Status.EmAndamento))
            {
                throw new InvalidOperationException("Não é possível excluir um projeto que possui tarefas pendentes ou em andamento. Primeiro conclua todas as tarefas ou as remova.");
            }

            projeto.DtDeleted = DateTime.Now;
            projeto.IsActive = false; // Marcar o projeto como inativo

            ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion Projeto



    #region Relatorio
    public decimal CalcularMediaTarefasConcluidas(string idUsuario)
    {
        // Obter a data de 30 dias atrás
        var dataInicio = DateTime.Today.AddDays(-30);

        // Consulta para calcular o número médio de tarefas concluídas por usuário nos últimos 30 dias
        var mediaTarefas = ctx.Tarefas
            .Count(t => t.UsuarioCriacao.Id == idUsuario && t.Status == Status.Concluida && t.DtConclucao >= dataInicio);

        return mediaTarefas;
    }
    #endregion Relatorio
}
