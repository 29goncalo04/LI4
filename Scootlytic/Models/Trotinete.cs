namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Trotinete
{
    [Key]
    public int IdTrotinete {get; set; }
    public string Modelo { get; set; }
    public string InformacaoTecnica { get; set; }
    public string Cor { get; set; }
    public int? NumeroEncomenda { get; set; }
    public Encomenda Encomenda { get; set; }
}