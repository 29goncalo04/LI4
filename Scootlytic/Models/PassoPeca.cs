namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class PassoPeca
{
    public int PassoId { get; set; }
    public int PecaReferencia { get; set; }
    public Passo Passo { get; set; }
    public Peca Peca { get; set; }
}