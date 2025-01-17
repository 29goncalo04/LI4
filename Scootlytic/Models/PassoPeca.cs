namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class PassoPeca
{
    public int PassoId { get; set; }    // Chave estrangeira para Passo
    public int PecaReferencia { get; set; } // Chave estrangeira para Peca

    // Propriedades de navegação
    public Passo Passo { get; set; }
    public Peca Peca { get; set; }
}