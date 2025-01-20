namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Possui
{
    [Key]
    public int IdTrotinete { get; set; }
    public int IdPasso { get; set; }
    public Trotinete Trotinete { get; set; }
    public Passo Passo { get; set; }
}