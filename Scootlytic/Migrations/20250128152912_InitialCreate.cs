﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scootlytic.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carrinhos",
                columns: table => new
                {
                    IdCarrinho = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorTotal = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrinhos", x => x.IdCarrinho);
                });

            migrationBuilder.CreateTable(
                name: "Passos",
                columns: table => new
                {
                    idPasso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroPasso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passos", x => x.idPasso);
                });

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    Referencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.Referencia);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Carrinhos_CartId",
                        column: x => x.CartId,
                        principalTable: "Carrinhos",
                        principalColumn: "IdCarrinho",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PassoPeca",
                columns: table => new
                {
                    PassoId = table.Column<int>(type: "int", nullable: false),
                    PecaReferencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassoPeca", x => new { x.PassoId, x.PecaReferencia });
                    table.ForeignKey(
                        name: "FK_PassoPeca_Passos_PassoId",
                        column: x => x.PassoId,
                        principalTable: "Passos",
                        principalColumn: "idPasso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassoPeca_Pecas_PecaReferencia",
                        column: x => x.PecaReferencia,
                        principalTable: "Pecas",
                        principalColumn: "Referencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Encomendas",
                columns: table => new
                {
                    Numero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetodoPagamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Condicao = table.Column<byte>(type: "tinyint", nullable: false),
                    EmailUtilizador = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encomendas", x => x.Numero);
                    table.ForeignKey(
                        name: "FK_Encomendas_Users_EmailUtilizador",
                        column: x => x.EmailUtilizador,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trotinetes",
                columns: table => new
                {
                    IdTrotinete = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformacaoTecnica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroEncomenda = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trotinetes", x => x.IdTrotinete);
                    table.ForeignKey(
                        name: "FK_Trotinetes_Encomendas_NumeroEncomenda",
                        column: x => x.NumeroEncomenda,
                        principalTable: "Encomendas",
                        principalColumn: "Numero",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Adicionada",
                columns: table => new
                {
                    IdCarrinho = table.Column<int>(type: "int", nullable: false),
                    IdTrotinete = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adicionada", x => new { x.IdCarrinho, x.IdTrotinete });
                    table.ForeignKey(
                        name: "FK_Adicionada_Carrinhos_IdCarrinho",
                        column: x => x.IdCarrinho,
                        principalTable: "Carrinhos",
                        principalColumn: "IdCarrinho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adicionada_Trotinetes_IdTrotinete",
                        column: x => x.IdTrotinete,
                        principalTable: "Trotinetes",
                        principalColumn: "IdTrotinete",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Escolhe",
                columns: table => new
                {
                    EmailUtilizador = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdTrotinete = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolhe", x => new { x.EmailUtilizador, x.IdTrotinete });
                    table.ForeignKey(
                        name: "FK_Escolhe_Trotinetes_IdTrotinete",
                        column: x => x.IdTrotinete,
                        principalTable: "Trotinetes",
                        principalColumn: "IdTrotinete",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolhe_Users_EmailUtilizador",
                        column: x => x.EmailUtilizador,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Possui",
                columns: table => new
                {
                    IdTrotinete = table.Column<int>(type: "int", nullable: false),
                    IdPasso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Possui", x => new { x.IdTrotinete, x.IdPasso });
                    table.ForeignKey(
                        name: "FK_Possui_Passos_IdPasso",
                        column: x => x.IdPasso,
                        principalTable: "Passos",
                        principalColumn: "idPasso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Possui_Trotinetes_IdTrotinete",
                        column: x => x.IdTrotinete,
                        principalTable: "Trotinetes",
                        principalColumn: "IdTrotinete",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adicionada_IdTrotinete",
                table: "Adicionada",
                column: "IdTrotinete");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_EmailUtilizador",
                table: "Encomendas",
                column: "EmailUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Escolhe_IdTrotinete",
                table: "Escolhe",
                column: "IdTrotinete");

            migrationBuilder.CreateIndex(
                name: "IX_PassoPeca_PecaReferencia",
                table: "PassoPeca",
                column: "PecaReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_Possui_IdPasso",
                table: "Possui",
                column: "IdPasso");

            migrationBuilder.CreateIndex(
                name: "IX_Trotinetes_NumeroEncomenda",
                table: "Trotinetes",
                column: "NumeroEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CartId",
                table: "Users",
                column: "CartId",
                unique: true,
                filter: "[CartId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adicionada");

            migrationBuilder.DropTable(
                name: "Escolhe");

            migrationBuilder.DropTable(
                name: "PassoPeca");

            migrationBuilder.DropTable(
                name: "Possui");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "Passos");

            migrationBuilder.DropTable(
                name: "Trotinetes");

            migrationBuilder.DropTable(
                name: "Encomendas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Carrinhos");
        }
    }
}
