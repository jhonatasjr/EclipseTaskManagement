using AutoMapper;
using EclipseWorks.API.Controllers.V1;
using EclipseWorks.API.Models;
using EclipseWorks.Domain.Enums;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Repositories.Base;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EclipseWorks.Test;
public class TarefaTest
{
    [Fact]
    public void CreateTask_ReturnsOkResult()
    {
        // Arrange
        var idProjeto = 1;
        var tarefaDTO = new TarefaDTO
        {
            Id = 50,
            IdUsuario = "1",
            Titulo = "Tarefa Teste",
            Descricao = "Descrição da tarefa teste",
            Prioridade = 1,
            Status = 1,
            DtVencimento = DateTime.Now.AddDays(7),
            Comentarios = new List<ComentarioDTO>
            {
                new ComentarioDTO { Texto = "Comentário 1" },
                new ComentarioDTO { Texto = "Comentário 2" }
            }
        };

        var tarefa = new Tarefa
        {
            Id = tarefaDTO.Id,
            IdUsuario = tarefaDTO.IdUsuario,
            Titulo = tarefaDTO.Titulo,
            Descricao = tarefaDTO.Descricao,
            Prioridade = (Prioridade)tarefaDTO.Prioridade,
            Status = (Status)tarefaDTO.Status,
            DtVencimento = tarefaDTO.DtVencimento,
            Comentarios = tarefaDTO.Comentarios.Select(c => new Comentario { Texto = c.Texto }).ToList()
        };

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.AdicionarTarefa(idProjeto, It.IsAny<Tarefa>()));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Tarefa>(tarefaDTO)).Returns(tarefa);
        mapperMock.Setup(mapper => mapper.Map<TarefaDTO>(tarefa)).Returns(tarefaDTO);

        var controller = new TarefaController(repositoryMock.Object, mapperMock.Object);

        // Act
        var result = controller.CreateTask(idProjeto, tarefaDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tarefaResult = Assert.IsType<TarefaDTO>(okResult.Value);
        Assert.NotNull(tarefaResult);
    }

    [Fact]
    public void AtualizarTarefa_TarefaExistente_RetornaOk()
    {
        // Arrange
        var idTarefa = 34;
        var idUsuario = "1";
        var tarefaUpd = new Tarefa
        {
            Descricao = "Nova descrição da tarefa",
            Status = Status.Concluida
        };

        var tarefa = new Tarefa
        {
            Id = idTarefa,
            Descricao = "Descrição atual da tarefa",
            Status = Status.EmAndamento
        };

        var repositoryMock = new Mock<IRepository>();

        repositoryMock.Setup(repo => repo.AtualizarTarefa(idTarefa, idUsuario, tarefaUpd))
                      .Callback<int, string, Tarefa>((id, userId, t) => {
                          tarefa.Descricao = t.Descricao;
                          tarefa.Status = t.Status;
                      });

        // Act
        try
        {
            repositoryMock.Object.AtualizarTarefa(idTarefa, idUsuario, tarefaUpd);

            // Assert
            Assert.Equal("Nova descrição da tarefa", tarefa.Descricao);
            Assert.Equal(Status.Concluida, tarefa.Status);
        }
        catch (KeyNotFoundException)
        {
            Assert.Fail("Tarefa não encontrada.");
        }
        catch (Exception ex)
        {
            Assert.Fail($"Erro ao atualizar a tarefa: {ex.Message}");
        }
    }

    [Fact]
    public void RemoveTask_ReturnsOkResult()
    {
        // Arrange
        var idTarefa = 1;

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.RemoverTarefa(idTarefa));

        var mapperMock = new Mock<IMapper>();
        var controller = new TarefaController(repositoryMock.Object, mapperMock.Object);

        // Act
        var result = controller.RemoveTask(idTarefa);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void AdicionarComentario_ReturnsOkResult()
    {
        // Arrange
        var idTarefa = 1;
        var comentarioDTO = new ComentarioDTO
        {
            Texto = "Novo comentário"
        };

        var comentario = new Comentario
        {
            Texto = comentarioDTO.Texto,
            TarefaId = idTarefa,
            IdUsuario = "1"
        };

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.AdicionarComentarioTarefa(It.IsAny<Comentario>()));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Comentario>(comentarioDTO)).Returns(comentario);

        var controller = new TarefaController(repositoryMock.Object, mapperMock.Object);

        // Act
        var result = controller.AdicionarComentario(idTarefa, comentarioDTO, "1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Comentário adicionado com sucesso.", okResult.Value);
    }

}
