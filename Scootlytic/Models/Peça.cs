namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Peca
{
    [Key]
    public int Referencia { get; set; } // Chave primária da peça
    public string Nome {get; set;}
    public byte Estado { get; set; } // Estado da peça, usando tinyint (0 a 255)
}