using AutoMapper;
using EclipseWorks.API.Models;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.API.AutoMapper;

public class ProjetoMapper : Profile
{
    public ProjetoMapper()
    {
        CreateMap<Projeto, ProjetoDTO>().ReverseMap();
        CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        CreateMap<Tarefa, TarefaDTO>().ReverseMap();
        CreateMap<Historico, HistoricoDTO>().ReverseMap();
        CreateMap<Comentario, ComentarioDTO>().ReverseMap();
        CreateMap<Tarefa, TarefaUpdateDTO>().ReverseMap();

    }
}
