namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Carrinho
{
    [Key]
    public int IdCarrinho { get; set; } // Chave primária

    [Column(TypeName = "decimal(6,2)")]
    public decimal ValorTotal { get; set; } // Valor total com até 6 dígitos e 2 casas decimais

    public User User { get; set; }

}
