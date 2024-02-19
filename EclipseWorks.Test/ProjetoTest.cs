using Moq;
using Microsoft.AspNetCore.Mvc;
using EclipseWorks.API.Models;
using EclipseWorks.Domain.Models;
using EclipseWorks.API.Controllers.V1;
using AutoMapper;
using EclipseWorks.Domain.Repositories.Base;
using EclipseWorks.Domain.Enums;

namespace EclipseWorks.Test
{
    public class ProjetoTest
    {
        //Adicionar Projeto
        [Fact]
        public void CreatProject_ReturnsOkResult()
        {
            // Arrange
            var projetoDto = new ProjetoDTO
            {
                Id = 50,
                NmProjeto = "Projeto Teste",
                IdUsuario = "1"
            };

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(repo => repo.AdicionarProjeto(It.IsAny<Projeto>()));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<Projeto>(projetoDto)).Returns(new Projeto());
            mapperMock.Setup(mapper => mapper.Map<ProjetoDTO>(It.IsAny<Projeto>())).Returns(projetoDto);

            var controller = new ProjetoController(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = controller.CreateProject(projetoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ProjetoDTO>(okResult.Value);
            Assert.NotNull(model);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(projetoDto, model);
        }

        //Listar todos os projetos por usuario
        [Fact]
        public void List_ReturnsOkResult()
        {
            // Arrange
            var idUsuario = "1";
            var usuario = new Usuario { Id = "1", Nome = "Usuário 1", Funcao = Funcao.Gerente }; // Criar uma instância válida de usuário

            var projetos = new List<Projeto>
            {
                new Projeto { Id = 1, NmProjeto = "Projeto 1", IdUsuario = idUsuario, CriadorDoProjeto = usuario },
                new Projeto { Id = 2, NmProjeto = "Projeto 2", IdUsuario = idUsuario, CriadorDoProjeto = usuario }
            };

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(repo => repo.ListarProjetos(idUsuario)).Returns(projetos);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<List<Projeto>>(projetos)).Returns(projetos);

            var controller = new ProjetoController(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = controller.List(idUsuario);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Projeto>>(okResult.Value);
            Assert.Equal(projetos.Count, model.Count);

            // Verifica se os detalhes dos projetos são mapeados corretamente
            for (int i = 0; i < projetos.Count; i++)
            {
                Assert.Equal(projetos[i].Id, model[i].Id);
                Assert.Equal(projetos[i].NmProjeto, model[i].NmProjeto);
                Assert.Equal(projetos[i].IdUsuario, model[i].IdUsuario);

                // Verifica se o nome do criador do projeto é mapeado corretamente
                Assert.Equal(projetos[i].CriadorDoProjeto?.Nome, model[i].CriadorDoProjeto?.Nome);
            }
        }


        //Remover Projeto
        [Fact]
        public void Delete_ReturnsOkResult()
        {
            // Arrange
            var idProjeto = 1;

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(repo => repo.RemoverProjeto(idProjeto));

            var mapperMock = new Mock<IMapper>();
            var controller = new ProjetoController(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = controller.Delete(idProjeto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFoundResult_WhenProjectNotFound()
        {
            // Arrange
            var idProjeto = 1;
            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(repo => repo.RemoverProjeto(idProjeto)).Throws(new KeyNotFoundException("Projeto não encontrado."));
            var mapperMock = new Mock<IMapper>();
            var controller = new ProjetoController(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = controller.Delete(idProjeto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Projeto não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void Delete_ReturnsBadRequestResult_WhenProjectHasPendingTasks()
        {
            // Arrange
            var idProjeto = 1;
            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(repo => repo.RemoverProjeto(idProjeto)).Throws(new InvalidOperationException("Não é possível excluir um projeto que possui tarefas pendentes ou em andamento. Primeiro conclua todas as tarefas ou as remova."));
            var mapperMock = new Mock<IMapper>();
            var controller = new ProjetoController(repositoryMock.Object, mapperMock.Object);
            // Act
            var result = controller.Delete(idProjeto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Não é possível excluir um projeto que possui tarefas pendentes ou em andamento. Primeiro conclua todas as tarefas ou as remova.", badRequestResult.Value);
        }

    }
}
