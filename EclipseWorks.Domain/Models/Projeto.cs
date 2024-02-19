using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorks.Domain.Models;
public class Projeto : BaseModel
{
    [Column(TypeName = "varchar(100)")]
    public string NmProjeto { get; set; } = string.Empty;

    [Column(TypeName = "varchar(100)")]
    public string IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario CriadorDoProjeto { get; set; }
    //public IList<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    public List<Tarefa> Tarefas { get; set; }

}