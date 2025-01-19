using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;  // Adiciona o namespace da sua aplicação
using Scootlytic.Models; // Adiciona o namespace do seu modelo User
using Microsoft.EntityFrameworkCore;
using System.Linq; // Para utilizar LINQ

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

            // Passo 1: Buscar os registros na tabela "Possui", ordenados por IdTrotinete
            var trotinetesEmMontagem = context.Possui
                .OrderBy(p => p.IdTrotinete) // Ordenar por IdTrotinete
                .ThenBy(p => p.IdPasso)
                .ToList();

            foreach (var trotinete in trotinetesEmMontagem)
            {
                // Passo 3: Buscar as associações na tabela "PassoPeca", ordenadas por PassoId
                var pecasAssociadas = context.PassoPeca
                    .Include(pp => pp.Peca)
                    .Where(pp => pp.PassoId == trotinete.IdPasso && pp.Peca.Estado == 1)
                    .OrderBy(pp => pp.PassoId)
                    .ThenBy(pp => pp.PecaReferencia)
                    .ToList(); // Pega o primeiro item ou null, caso não haja nenhum

                // Simula o tempo de montagem com um delay aleatório entre 10 e 30 segundos
                var delay = new Random().Next(10000, 30000);
                await Task.Delay(delay, stoppingToken); // Simulação do tempo de montagem

                // Caso seja o passo 5, buscamos um "brake" e duas "wheels" para remover as linhas específicas
                if (trotinete.IdPasso == 5)
                {
                    var brakes = pecasAssociadas.Where(pp => pp.Peca.Nome == "Brakes").ToList();
                    var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();

                    if (brakes.Count >= 1 && wheels.Count >= 2)
                    {
                        // Remover as linhas específicas da tabela PassoPeca
                        var passoPecaAssociadoBrake = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Brakes")
                            .FirstOrDefault();
                        var passoPecaAssociadoWheel1 = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels")
                            .FirstOrDefault();
                        var passoPecaAssociadoWheel2 = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels")
                            .OrderBy(pp => pp.PassoId)
                            .Skip(1)  // Pega a segunda wheel, caso exista
                            .FirstOrDefault();

                        // Remover as peças associadas
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

                // Caso seja o passo 6, buscamos "Lights" para remover as linhas específicas
                if (trotinete.IdPasso == 6)
                {
                    var lights = pecasAssociadas.Where(pp => pp.Peca.Nome == "Lights").ToList();

                    if (lights.Count >= 2)
                    {
                        var passoPecaAssociadoLights = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Lights" && pp.PassoId == trotinete.IdPasso)
                            .ToList();
                        
                        // Remover as peças associadas
                        if (passoPecaAssociadoLights.Any())
                        {
                            context.PassoPeca.RemoveRange(passoPecaAssociadoLights);
                        }
                    }
                }

                // Caso seja o passo 8, buscamos "Wheels" para remover as linhas específicas
                if (trotinete.IdPasso == 8)
                {
                    var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();

                    if (wheels.Count >= 2)
                    {
                        var passoPecaAssociadoWheels = context.PassoPeca
                            .Where(pp => pp.Peca.Nome == "Wheels" && pp.PassoId == trotinete.IdPasso)
                            .ToList();
                        
                        // Remover as peças associadas
                        if (passoPecaAssociadoWheels.Any())
                        {
                            context.PassoPeca.RemoveRange(passoPecaAssociadoWheels);
                        }
                    }
                }

                // Remover a linha da tabela "Possui" e "PassoPeca" de forma geral
                context.Possui.Remove(trotinete);
                var passoPecaAssociadoGeral = context.PassoPeca
                    .Where(pp => pp.PassoId == trotinete.IdPasso);
                context.PassoPeca.RemoveRange(passoPecaAssociadoGeral);

                // Salvar as alterações no banco de dados
                await context.SaveChangesAsync(stoppingToken);
            }
        }

        // Pequeno delay para evitar sobrecarga na consulta ao banco
        await Task.Delay(100, stoppingToken);
    }
}



private async Task RemoverTrotineteDepoisDelayAsync(Possui trotinete, ApplicationDbContext context, CancellationToken stoppingToken)
{
    // Simula o tempo de montagem com um delay aleatório entre 10 e 30 segundos
    var delay = new Random().Next(10000, 30000); // Tempo aleatório entre 10 e 30 segundos
    await Task.Delay(delay, stoppingToken); // Simulação do tempo de montagem

    // Remover da tabela "Possui"
    context.Possui.Remove(trotinete);

    // Salvar as alterações após a remoção
    await context.SaveChangesAsync(stoppingToken); // Salva a alteração no banco
}


}
