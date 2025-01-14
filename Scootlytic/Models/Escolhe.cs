namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Escolhe
{
    [Key]
    public string EmailUtilizador { get; set; } // Chave primária que aponta para o email do utilizador
    [Key]
    public string ModeloTrotinete { get; set; } // Chave primária que aponta para o modelo da trotinete
    // Propriedade de navegação para User
    public User User { get; set; }
    // Propriedade de navegação para Trotinete
    public Trotinete Trotinete { get; set; }
}