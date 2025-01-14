namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Passo
{
    [Key]
    public int idPasso { get; set; } // Chave primária do passo
    public int NumeroPasso { get; set; } // Número do passo (pode ser um número sequencial)
    [StringLength(50)] // A referência para a peça deve ter até 50 caracteres
    public string ReferenciaPeca { get; set; } // Chave estrangeira que aponta para a tabela Peça
    public Peca Peca { get; set; } // Navegação para a tabela Peça (opcional)
}