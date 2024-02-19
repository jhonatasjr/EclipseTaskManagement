using System.ComponentModel.DataAnnotations.Schema;
using EclipseWorks.Domain.Enums;

namespace EclipseWorks.Domain.Models;
public class Usuario
{
    [Column(TypeName = "varchar(200)")]
    public string Id { get; set; }
    [Column(TypeName = "varchar(200)")]
    public string Nome { get; set; }
    public Funcao Funcao { get; set; }
}