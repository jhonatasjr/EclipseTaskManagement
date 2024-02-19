using EclipseWorks.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorks.Domain.Models;
public class Tarefa : BaseModel
{
    public string Titulo { get; set; }

    [Column(TypeName = "varchar(250)")]
    public string Descricao { get; set; }
    public DateTime DtVencimento { get; set; }
    public DateTime? DtConclucao { get; set; }
    public Status Status { get; set; }
    public Prioridade Prioridade { get; set; }

    public int IdProjeto { get; set; }

    [ForeignKey("IdProjeto")]
    public Projeto Projeto { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public Usuario UsuarioCriacao { get; set; }

    //public List<Comentario> Comentarios { get; set; }
    public IList<Comentario> Comentarios { get; set; } = new List<Comentario>();


    public void SetStatus(Status status)
    {
        Status = status;

        if (Status == Status.Concluida)
        {
            DtConclucao = DateTime.Now;
        }
    }
}