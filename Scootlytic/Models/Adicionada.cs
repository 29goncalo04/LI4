namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Adicionada
{
    [Key]
    public int IdCarrinho { get; set; }
    [Key]
    public int IdTrotinete { get; set; }
    public Carrinho Carrinho { get; set; }
    public Trotinete Trotinete { get; set; }
}