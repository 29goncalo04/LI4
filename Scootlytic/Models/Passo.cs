namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Passo
{
    [Key]
    public int idPasso { get; set; }
    public int NumeroPasso { get; set; }
}