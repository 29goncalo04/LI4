namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Encomenda
{
    [Key]
    public int Numero { get; set; } // Chave primária
    public DateTime DataEntrega { get; set; }
    public string MetodoPagamento { get; set; }
    public byte Condicao { get; set; } // Tinyint
    public string EmailUtilizador { get; set; } // Chave estrangeira
    
    // Definição da chave estrangeira
    public User User { get; set; } // Propriedade de navegação
}