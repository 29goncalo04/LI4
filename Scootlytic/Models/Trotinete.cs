namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Trotinete
{
    [Key]
    public int IdTrotinete {get; set; }
    public string Modelo { get; set; } // Chave primária do modelo (VARCHAR(100))
    
    public string InformacaoTecnica { get; set; } // Informações técnicas (TEXT)
    
    public string Cor { get; set; } // Cor da trotinete
    
    public int NumeroEncomenda { get; set; } // Chave estrangeira para Encomenda
    // Propriedade de navegação para Encomenda
    public Encomenda Encomenda { get; set; } // Relacionamento com a tabela Encomenda
}