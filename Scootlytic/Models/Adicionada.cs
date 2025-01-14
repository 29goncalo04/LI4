namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Adicionada
{
    [Key]
    public int IdCarrinho { get; set; } // Chave primária que aponta para o carrinho
    [Key]
    public int IdTrotinete { get; set; } // Chave primária que aponta para o modelo da trotinete
    // Propriedade de navegação para Carrinho
    public Carrinho Carrinho { get; set; }
    // Propriedade de navegação para Trotinete
    public Trotinete Trotinete { get; set; }
}