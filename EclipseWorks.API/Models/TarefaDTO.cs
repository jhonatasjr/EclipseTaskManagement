using EclipseWorks.Domain.Models;

namespace EclipseWorks.API.Models;

public class TarefaDTO
{
    public int Id { get; set; }
    public string IdUsuario { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public int Prioridade { get; set; }
    public int Status { get; set; }
    public DateTime DtVencimento { get; set; }
    public List<ComentarioDTO> Comentarios { get; set; }

}

public class ComentarioDTO
{
    public string Texto { get; set; }
}