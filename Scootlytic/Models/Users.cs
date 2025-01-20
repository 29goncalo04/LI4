namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    [Key]
    public string Email { get; set; }
    public string Password { get; set; }
    public int? CartId { get; set; }
    public Carrinho Carrinho { get; set; }
}