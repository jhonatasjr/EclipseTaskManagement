using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EclipseWorks.Domain.Models;
public class Comentario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string Texto { get; set; }
    public int TarefaId { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string IdUsuario { get; set; }
    public DateTime DtCreate { get; set; }
    //public Tarefa Tarefa { get; set; }

    public Comentario()
    {
        DtCreate = DateTime.Now;
    }
}
