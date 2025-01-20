namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Peca
{
    [Key]
    public int Referencia { get; set; }
    public string Nome {get; set;}
    public byte Estado { get; set; }
}