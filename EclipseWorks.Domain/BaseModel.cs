using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclipseWorks.Domain;
public class BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime DtCreate { get; set; }
    public string UserAlter { get; set; } = string.Empty;
    public DateTime? DtAlter { get; set; }
    public string UserDeleted { get; set; } = string.Empty;
    public DateTime? DtDeleted { get; set; }

    public BaseModel()
    {
        IsActive = true;
        DtCreate = DateTime.Now;
    }
}
