using AutoMapper;
using EclipseWorks.API.Models;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers.V1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/projeto")]
[ApiController]
public class ProjetoController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    //Construtor
    public ProjetoController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    //Listar todos os projetos por usuario
    [HttpGet("{IdUsuario}")]
    public IActionResult List([FromRoute] string IdUsuario)
    {
        try
        {
            var projetos = _repository.ListarProjetos(IdUsuario);
            var projetosDTO = _mapper.Map<List<Projeto>>(projetos);
            return Ok(projetosDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }

    //Adicionar Projeto
    [HttpPost]
    public IActionResult CreateProject(ProjetoDTO projetoDto)
    {
        try
        {
            var projeto = _mapper.Map<Projeto>(projetoDto);
            _repository.AdicionarProjeto(projeto);
            var projetosDTO = _mapper.Map<ProjetoDTO>(projeto);
            return Ok(projetosDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }

    //Remover Projeto
    [HttpDelete("{IdProjeto:int}")]
    public IActionResult Delete(int IdProjeto)
    {
        try
        {
            _repository.RemoverProjeto(IdProjeto);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao processar a solicitação: {ex.Message}");
        }
    }

}