using Microsoft.EntityFrameworkCore;
using Scootlytic.Models;
using Scootlytic.Data;

var builder = WebApplication.CreateBuilder(args);


// Configurar a string de conexão com a base de dados (Azure ou local)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Aplicar migrações automaticamente ao iniciar a aplicação, caso esteja em desenvolvimento
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();  // Aplica automaticamente as migrações pendentes
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Serve arquivos estáticos (CSS, JS, imagens) da pasta wwwroot
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Admin}/{action=Admin}/{id?}");


app.Run();
