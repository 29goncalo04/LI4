namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Encomenda
{
    [Key]
    public int Numero { get; set; }
    public DateTime DataEntrega { get; set; }
    public string MetodoPagamento { get; set; }
    public byte Condicao { get; set; }
    public string EmailUtilizador { get; set; }
    public User User { get; set; }
}