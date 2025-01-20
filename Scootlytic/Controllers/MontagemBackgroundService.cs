using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;
using Scootlytic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class MontagemBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MontagemBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var trotinetesEmMontagem = context.Possui
                .OrderBy(p => p.IdTrotinete)
                .ThenBy(p => p.IdPasso)
                .ToList();

            foreach (var trotinete in trotinetesEmMontagem)
            {
                var pecasAssociadas = context.PassoPeca
                    .Include(pp => pp.Peca)
                    .Where(pp => pp.PassoId == trotinete.IdPasso && pp.Peca.Estado == 1)
                    .OrderBy(pp => pp.PassoId)
                    .ThenBy(pp => pp.PecaReferencia)
                    .ToList();
                var delay = new Random().Next(10000, 30000);
                await Task.Delay(delay, stoppingToken);

                if (trotinete.IdPasso == 5)
                {
                    var brakes = pecasAssociadas.Where(pp => pp.Peca.Nome == "Brakes").ToList();
                    var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();

                    if (brakes.Count >= 1 && wheels.Count >= 2)
                    {
                        var passoPecaAssociadoBrake = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Brakes")
                            .FirstOrDefault();
                        var passoPecaAssociadoWheel1 = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels")
                            .FirstOrDefault();
                        var passoPecaAssociadoWheel2 = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels")
                            .OrderBy(pp => pp.PassoId)
                            .Skip(1)
                            .FirstOrDefault();
                        if (passoPecaAssociadoBrake != null)
                        {
                            context.PassoPeca.Remove(passoPecaAssociadoBrake);
                        }
                        if (passoPecaAssociadoWheel1 != null)
                        {
                            context.PassoPeca.Remove(passoPecaAssociadoWheel1);
                        }
                        if (passoPecaAssociadoWheel2 != null)
                        {
                            context.PassoPeca.Remove(passoPecaAssociadoWheel2);
                        }
                    }
                }

                if (trotinete.IdPasso == 6)
                {
                    var lights = pecasAssociadas.Where(pp => pp.Peca.Nome == "Lights").ToList();

                    if (lights.Count >= 2)
                    {
                        var passoPecaAssociadoLights = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Lights" && pp.PassoId == trotinete.IdPasso)
                            .ToList();
                        
                        if (passoPecaAssociadoLights.Any())
                        {
                            context.PassoPeca.RemoveRange(passoPecaAssociadoLights);
                        }
                    }
                }
                if (trotinete.IdPasso == 8)
                {
                    var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();

                    if (wheels.Count >= 2)
                    {
                        var passoPecaAssociadoWheels = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels" && pp.PassoId == trotinete.IdPasso)
                            .ToList();

                        if (passoPecaAssociadoWheels.Any())
                        {
                            context.PassoPeca.RemoveRange(passoPecaAssociadoWheels);
                        }
                    }
                }
                context.Possui.Remove(trotinete);
                var passoPecaAssociadoGeral = context.PassoPeca
                    .Where(pp => pp.PassoId == trotinete.IdPasso);
                context.PassoPeca.RemoveRange(passoPecaAssociadoGeral);
                await context.SaveChangesAsync(stoppingToken);
            }
        }
        await Task.Delay(100, stoppingToken);
    }
}



private async Task RemoverTrotineteDepoisDelayAsync(Possui trotinete, ApplicationDbContext context, CancellationToken stoppingToken)
{
    var delay = new Random().Next(10000, 30000);
    await Task.Delay(delay, stoppingToken);
    context.Possui.Remove(trotinete);
    await context.SaveChangesAsync(stoppingToken);
}

}
