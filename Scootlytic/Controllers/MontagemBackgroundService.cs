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
    
                    if (trotinete.IdPasso == 5)
                    {
                        // Caso seja o passo 5, buscamos um "brake" e duas "wheels"
                        var brakes = pecasAssociadas.Where(pp => pp.Peca.Nome == "Brakes").ToList();
                        var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();
    
                        if (brakes.Count >= 1 && wheels.Count >= 2)
                        {
                            // Processa o brake e as duas wheels
                            await ProcessarPecaEmMontagemAsync(brakes[0].Peca, context, stoppingToken);
                            await ProcessarPecaEmMontagemAsync(wheels[0].Peca, context, stoppingToken);
                            await ProcessarPecaEmMontagemAsync(wheels[1].Peca, context, stoppingToken);
                        }
                    }
                    if (trotinete.IdPasso == 6)
                    {
                        var lights = pecasAssociadas.Where(pp => pp.Peca.Nome == "Lights").ToList();
    
                        if (lights.Count >= 2)
                        {
                            await ProcessarPecaEmMontagemAsync(lights[0].Peca, context, stoppingToken);
                            await ProcessarPecaEmMontagemAsync(lights[1].Peca, context, stoppingToken);
                        }
                    }
                    if (trotinete.IdPasso == 8)
                    {
                        var wheels = pecasAssociadas.Where(pp => pp.Peca.Nome == "Wheels").ToList();
    
                        if (wheels.Count >= 2)
                        {
                            await ProcessarPecaEmMontagemAsync(wheels[0].Peca, context, stoppingToken);
                            await ProcessarPecaEmMontagemAsync(wheels[1].Peca, context, stoppingToken);
                        }
                    }
                    else {
                        // Para outros passos, processa a primeira peça encontrada
                        var passoPeca = pecasAssociadas.FirstOrDefault();
                        if (passoPeca != null && passoPeca.Peca != null)
                        {
                            var peca = passoPeca.Peca; // Acessa a peça associada ao passo
                            await ProcessarPecaEmMontagemAsync(peca, context, stoppingToken);
                        }
                    }
                }
            }
    
            // Pequeno delay para evitar sobrecarga na consulta ao banco
            await Task.Delay(100, stoppingToken);
        }
    }

private async Task ProcessarPecaEmMontagemAsync(Peca peca, ApplicationDbContext context, CancellationToken stoppingToken)
{
        // Simula o tempo de montagem com um delay aleatório entre 10 e 30 segundos
        var delay = new Random().Next(10000, 30000);
        await Task.Delay(delay, stoppingToken); // Simulação do tempo de montagem

        // Atualiza o estado da peça para "montada"
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            peca.Estado = 2;
            scopedContext.Pecas.Update(peca);
            await scopedContext.SaveChangesAsync(stoppingToken); // Salva a alteração no banco
        }
}

}
