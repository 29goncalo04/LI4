using Microsoft.EntityFrameworkCore;
using Scootlytic.Models;
using Scootlytic.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHostedService<MontagemBackgroundService>(); 

builder.Services.AddControllersWithViews();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();

        if (!context.Passos.Any())
        {
            var passos = new List<Passo>
            {
                new Passo { NumeroPasso = 1 },
                new Passo { NumeroPasso = 2 },
                new Passo { NumeroPasso = 3 },
                new Passo { NumeroPasso = 4 },
                new Passo { NumeroPasso = 5 },
                new Passo { NumeroPasso = 6 },
                new Passo { NumeroPasso = 1 },
                new Passo { NumeroPasso = 2 }
            };

            context.Passos.AddRange(passos);
            context.SaveChanges();
        }

        if (!context.Pecas.Any())
        {
            var pecas = new List<Peca>
            {
                new Peca { Nome = "Batteries", Estado = 0},
                new Peca { Nome = "Batteries", Estado = 0},
                new Peca { Nome = "Batteries", Estado = 0},
                new Peca { Nome = "Brakes", Estado = 0},
                new Peca { Nome = "Brakes", Estado = 0},
                new Peca { Nome = "Brakes", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Lights", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Wheels", Estado = 0},
                new Peca { Nome = "Motors", Estado = 0},
                new Peca { Nome = "Motors", Estado = 0},
                new Peca { Nome = "Motors", Estado = 0},
                new Peca { Nome = "Control Screens", Estado = 0},
                new Peca { Nome = "Control Screens", Estado = 0},
                new Peca { Nome = "Control Screens", Estado = 0},
                new Peca { Nome = "Frames", Estado = 0},
                new Peca { Nome = "Frames", Estado = 0},
                new Peca { Nome = "Frames", Estado = 0}
            };
            context.Pecas.AddRange(pecas);
            context.SaveChanges();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Admin}/{action=Admin}/{id?}");

app.Run();