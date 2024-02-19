using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorks.Domain.Models;
public class Historico
{
    public int Id { get; set; }
    public int IdTarefa { get; set; }

    [Column(TypeName = "varchar(200)")]
    public string TpModificacao { get; set; }
    
    [Column(TypeName = "varchar(500)")]
    public string Alteracoes { get; set; }
    public DateTime DtModificacao { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public string UserAlter { get; set; }
}
