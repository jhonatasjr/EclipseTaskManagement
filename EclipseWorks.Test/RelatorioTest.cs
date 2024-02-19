using AutoMapper;
using EclipseWorks.API.Controllers.V1;
using EclipseWorks.Domain.Repositories.Base;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EclipseWorks.Test;
public class RelatorioTest
{
    [Fact]
    public void MediaTasksCompleted_ReturnsOkResult()
    {
        // Arrange
        var idUsuario = "1";
        var mediaEsperada = 5;

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.CalcularMediaTarefasConcluidas(idUsuario)).Returns(mediaEsperada);

        var mapperMock = new Mock<IMapper>();
        var controller = new RelatorioController(repositoryMock.Object, mapperMock.Object);

        // Act
        var result = controller.MediaTasksCompleted(idUsuario);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var mediaRetornada = Assert.IsType<decimal>(okResult.Value);
        Assert.Equal(mediaEsperada, mediaRetornada);
        Assert.Equal(200, okResult.StatusCode);
    }
}
