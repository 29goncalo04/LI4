namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Passo
{
    [Key]
    public int idPasso { get; set; } // Chave primária do passo
    public int NumeroPasso { get; set; } // Número do passo (pode ser um número sequencial)
}