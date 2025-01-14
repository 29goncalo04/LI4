namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    [Key]
    public string Email { get; set; }  // Alterado para ser a chave primária
    public string Password { get; set; }
    public int? CartId { get; set; }  // Novo campo CartId

    public Carrinho Carrinho { get; set; }
}