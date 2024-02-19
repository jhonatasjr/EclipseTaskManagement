using AutoMapper;
using EclipseWorks.API.Models;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers.V1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    //Construtor
    public TarefaController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    //Adicionar Tarefa
    [HttpPost("{IdProjeto:int}")]
    public IActionResult CreateTask(int IdProjeto, TarefaDTO tarefaDTO)
    {
        try
        {
            var tarefa = _mapper.Map<Tarefa>(tarefaDTO);
            _repository.AdicionarTarefa(IdProjeto, tarefa);
            var projetosDTO = _mapper.Map<TarefaDTO>(tarefa);
            return Ok(projetosDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }


    //Atualizar Tarefa
    [HttpPut("{IdTarefa:int}")]
    public IActionResult UpdateTask(int IdTarefa, [FromBody] TarefaUpdateDTO tarefaUpdateDTO, [FromQuery] string IdUsuario)
    {
        try
        {
            // Verifica se o IdUsuario não é nulo ou vazio
            if (string.IsNullOrEmpty(IdUsuario))
            {
                return BadRequest("O ID do usuário não pode estar vazio.");
            }

            var tarefa = _mapper.Map<Tarefa>(tarefaUpdateDTO);
            _repository.AtualizarTarefa(IdTarefa, IdUsuario, tarefa);
            return Ok(tarefaUpdateDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }


    //Remover Tarefa
    [HttpDelete("{IdTarefa:int}")]
    public IActionResult RemoveTask(int IdTarefa)
    {
        try
        {
            _repository.RemoverTarefa(IdTarefa);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }


    [HttpPost("{idTarefa:int}/comentarios")]
    public IActionResult AdicionarComentario(int idTarefa, ComentarioDTO comentarioDTO, string IdUsuario)
    {
        try
        {
            var comentario = _mapper.Map<Comentario>(comentarioDTO);
            comentario.TarefaId = idTarefa;
            comentario.IdUsuario = IdUsuario;
            _repository.AdicionarComentarioTarefa(comentario);
            return Ok("Comentário adicionado com sucesso.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }

}
