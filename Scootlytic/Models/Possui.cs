namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Possui
{
    [Key]
    public int IdTrotinete { get; set; } // Chave estrangeira para a tabela Trotinete
    public int IdPasso { get; set; } // Chave estrangeira para a tabela Passo
    public Trotinete Trotinete { get; set; } // Navegação para a tabela Trotinete (opcional)
    public Passo Passo { get; set; } // Navegação para a tabela Passo (opcional)
}