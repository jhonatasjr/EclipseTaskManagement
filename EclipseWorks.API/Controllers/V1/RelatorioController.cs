using AutoMapper;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers.V1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/relatorio")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    //Construtor
    public RelatorioController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("media-tarefas-concluidas")]
    public IActionResult MediaTasksCompleted([FromQuery] string IdUsuario)
    {
        try
        {
            var mediaTarefas = _repository.CalcularMediaTarefasConcluidas(IdUsuario);

            return Ok(mediaTarefas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }

}
