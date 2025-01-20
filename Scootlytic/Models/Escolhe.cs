namespace Scootlytic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Escolhe
{
    [Key]
    public string EmailUtilizador { get; set; }
    [Key]
    public int IdTrotinete { get; set; }
    public User User { get; set; }
    public Trotinete Trotinete { get; set; }
}