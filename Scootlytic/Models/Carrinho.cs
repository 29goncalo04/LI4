namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Carrinho
{
    [Key]
    public int IdCarrinho { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal ValorTotal { get; set; }
    public User User { get; set; }

}
