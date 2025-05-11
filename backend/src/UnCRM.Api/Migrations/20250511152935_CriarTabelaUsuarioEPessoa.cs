using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UnCRM.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaUsuarioEPessoa : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pessoa");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
