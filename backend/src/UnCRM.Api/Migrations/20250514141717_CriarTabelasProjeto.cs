using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnCRM.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pessoa",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: false),
                    NomeCurto = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: false),
                    TipoPessoa = table.Column<string>(type: "VARCHAR", maxLength: 15, nullable: false),
                    CpfCnpj = table.Column<string>(type: "VARCHAR", maxLength: 14, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_atendimento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_atendimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "VARCHAR", maxLength: 20, nullable: false),
                    Senha = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: false),
                    Cargo = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "atendimento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: false),
                    TipoAtendimentoId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioCriacaoId = table.Column<long>(type: "BIGINT", nullable: false),
                    PessoaId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioProximoContato = table.Column<long>(type: "bigint", nullable: true),
                    DataProximoContato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_atendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_atendimento_pessoa_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_atendimento_tipo_atendimento_TipoAtendimentoId",
                        column: x => x.TipoAtendimentoId,
                        principalTable: "tipo_atendimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parecer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioCriacaoId = table.Column<long>(type: "BIGINT", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    AtendimentoId = table.Column<long>(type: "BIGINT", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parecer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parecer_atendimento_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "atendimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tipo_atendimento",
                columns: new[] { "Id", "Descricao" },
                values: new object[,]
                {
                    { 1L, "Renovação de Contrato" },
                    { 2L, "Atualização de Sistema" },
                    { 3L, "Dúvida sobre o sistema" },
                    { 4L, "Erro de Sistema" },
                    { 5L, "Reclamação" },
                    { 6L, "Cancelamento" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_PessoaId",
                table: "atendimento",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_TipoAtendimentoId",
                table: "atendimento",
                column: "TipoAtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_parecer_AtendimentoId",
                table: "parecer",
                column: "AtendimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parecer");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "atendimento");

            migrationBuilder.DropTable(
                name: "pessoa");

            migrationBuilder.DropTable(
                name: "tipo_atendimento");
        }
    }
}
