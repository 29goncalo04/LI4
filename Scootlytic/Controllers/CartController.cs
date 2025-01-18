using Microsoft.AspNetCore.Mvc;
using Scootlytic.Models; // Substitua pelo seu namespace
using Scootlytic.Data; // Substitua pelo seu contexto de banco de dados
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Scootlytic.Cart
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Paypal()
        {
            return View();
        }
        public IActionResult Mbway()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View(); // A página inicial será carregada sem dados
        }



        [HttpGet]  // Ou [HttpPost] se preferir, mas o padrão é GET para essa operação
        public IActionResult GetCartItems()
        {
            var userEmail = Request.Headers["User-Email"].ToString(); // Pega o email do header

            // Busca o carrinho do usuário
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { success = false, message = "Carrinho não encontrado." });
            }

            // Agora fazemos o join com a tabela Trotinete
            var itensCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete, // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete, // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Adicionada = a, Trotinete = t } // Seleciona os dados que você precisa
                )
                .ToList();
            // Retorna os dados como JSON
            return Json(new { success = true, items = itensCarrinho });
        }


        [HttpGet]  // Usar GET para a remoção
        public IActionResult RemoveFromCart(string modelo)
        {
            var userEmail = Request.Headers["User-Email"].ToString(); // Pega o email do header
            Console.WriteLine(modelo);

            // Encontrar o carrinho do usuário
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { error = "Carrinho não encontrado." });
            }

            // Buscar todas as trotinetes do modelo especificado associadas ao carrinho
            var itens = _context.Adicionada
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete,  // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete,  // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Adicionada = a, Trotinete = t } // Seleciona dados de Adicionada e Trotinete
                )
                .Where(a => a.Adicionada.IdCarrinho == carrinho.IdCarrinho && a.Trotinete.Modelo == modelo)
                .ToList();

            if (itens.Count == 0)
            {
                Console.WriteLine($"Nenhuma trotinete com o modelo {modelo} encontrada no carrinho.");
                return Json(new { error = "Nenhuma trotinete do modelo especificado encontrada no carrinho." });
            }

            // Remover todas as trotinetes do modelo especificado da tabela Adicionada
            _context.Adicionada.RemoveRange(itens.Select(i => i.Adicionada));
            _context.SaveChanges();

            // Verificar se ainda existem trotinetes do modelo especificado em outros carrinhos
            var trotineteIds = itens.Select(i => i.Trotinete.IdTrotinete).Distinct().ToList();
            var trotinetesNaoAssociadas = _context.Adicionada
                .Any(a => trotineteIds.Contains(a.IdTrotinete));

            // Se não houverem mais registros de trotinetes associadas ao modelo, remover da tabela Trotinetes
            if (!trotinetesNaoAssociadas)
            {
                var trotinetesARemover = _context.Trotinetes
                    .Where(t => trotineteIds.Contains(t.IdTrotinete))
                    .ToList();

                _context.Trotinetes.RemoveRange(trotinetesARemover);
                _context.SaveChanges();
            }

            // Recalcular o total do carrinho após a remoção
            var itemsCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete,  // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete,  // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Trotinete = t } // Seleciona apenas a Trotinete
                )
                .ToList();

            // Calcular o novo valor total
            decimal newTotalPrice = itemsCarrinho
                .Sum(item => item.Trotinete.Modelo == "SPEEDY Electric Scooter" ? 79.99m : item.Trotinete.Modelo == "GLIDY Scooter" ? 49.99m : 0);

            // Atualizar o valor total do carrinho na tabela Carrinho
            carrinho.ValorTotal = newTotalPrice;
            _context.SaveChanges();

            // Retorna o novo total
            return Json(new { newTotalPrice });
        }

        [HttpPost]
        public IActionResult FinalizarCompra([FromBody] Dictionary<string, string> requestData)        {
            var userEmail = Request.Headers["User-Email"].ToString();
            requestData.TryGetValue("MetodoPagamento", out string metodoPagamento);

            // Encontrar o carrinho do usuário
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { error = "Carrinho não encontrado." });
            }

            // Buscar todas as trotinetes associadas ao carrinho
            var trotinetesNoCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Select(a => a.Trotinete)
                .ToList();

            if (trotinetesNoCarrinho.Count == 0)
            {
                return Json(new { error = "O carrinho está vazio." });
            }

            // Criar uma nova encomenda associada ao usuário
            var novaEncomenda = new Encomenda
            {
                DataEntrega = DateTime.UtcNow.AddDays(3), // Simulação de prazo de entrega
                MetodoPagamento = metodoPagamento,
                Condicao = 1, // Status de encomenda (1 = processando, por exemplo)
                EmailUtilizador = userEmail
            };

            _context.Encomendas.Add(novaEncomenda);
            _context.SaveChanges(); // Salvar para obter o ID da encomenda

            // Atualizar as trotinetes para apontarem para o ID da nova encomenda
            foreach (var trotinete in trotinetesNoCarrinho)
            {
                trotinete.NumeroEncomenda = novaEncomenda.Numero;
            
                // Determinar os passos e peças necessárias
                var passosNecessarios = trotinete.Modelo switch
                {
                    "GLIDY Scooter" => new List<(int passo, string peca)>
                    {
                        (7, "Frames"),
                        (8, "Wheels")
                    },
                    "SPEEDY Electric Scooter" => new List<(int passo, string peca)>
                    {
                        (1, "Frames"),
                        (2, "Motors"),
                        (3, "Batteries"),
                        (4, "Control Screens"),
                        (5, "Wheels"),
                        (5, "Brakes"),
                        (6, "Lights")
                    },
                    _ => new List<(int passo, string peca)>()
                };
            
                foreach (var (idPasso, nomePeca) in passosNecessarios)
                {
                    // Determinar a quantidade necessária (2 para Lights/Wheels, 1 para o restante)
                    int quantidadeNecessaria = (nomePeca == "Lights" || nomePeca == "Wheels") ? 2 : 1;
            
                    for (int i = 0; i < quantidadeNecessaria; i++)
                    {
                        // Procurar a primeira peça disponível com estado = 0
                        var pecaDisponivel = _context.Pecas
                            .FirstOrDefault(p => p.Nome == nomePeca && p.Estado == 0);
            
                        if (pecaDisponivel == null)
                        {
                            // Criar 10 novas peças se não houver disponíveis
                            var novasPecas = Enumerable.Range(0, 10)
                                .Select(_ => new Peca { Nome = nomePeca, Estado = 0 })
                                .ToList();
                            _context.Pecas.AddRange(novasPecas);
                            _context.SaveChanges();
            
                            // Buscar novamente uma peça disponível
                            pecaDisponivel = novasPecas.First();
                        }
            
                        // Verificar se o registro já existe na tabela PassoPeca
                        var existePassoPeca = _context.PassoPeca
                            .AsNoTracking() // Evita rastreamento das entidades carregadas
                            .Any(pp => pp.PassoId == idPasso && pp.PecaReferencia == pecaDisponivel.Referencia);
            
                        if (!existePassoPeca)
                        {
                            // Associar a peça ao passo se ainda não estiver associada
                            var passoPeca = new PassoPeca
                            {
                                PassoId = idPasso,
                                PecaReferencia = pecaDisponivel.Referencia
                            };
                            _context.PassoPeca.Add(passoPeca); // Adicionar a associação
                        }
            
                        // Atualizar o estado da peça para 1 (utilizada)
                        pecaDisponivel.Estado = 1;
            
                        // Anexar peça ao contexto, se necessário, para evitar duplicatas
                        _context.Pecas.Attach(pecaDisponivel);
                        _context.SaveChanges();
                    }
                }
            }
            
            // Salvar todas as alterações após o loop principal
            _context.SaveChanges();


            // Remover as associações do carrinho na tabela Adicionada
            var itensAdicionados = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .ToList();

            _context.Adicionada.RemoveRange(itensAdicionados);

            // Atualizar o valor total do carrinho para 0
            carrinho.ValorTotal = 0;

            _context.SaveChanges(); // Salvar todas as alterações

            return Json(new { success = true, message = "Pagamento efetuado e encomenda criada." });
        }
    }
}