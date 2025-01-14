namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Peca
{
    [Key]
    [StringLength(50)] // Limite de 50 caracteres para a referência
    public string Referencia { get; set; } // Chave primária da peça
    public byte Estado { get; set; } // Estado da peça, usando tinyint (0 a 255)
}