using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data.Mappings
{
    public class PessoaMap : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("pessoa")
            .HasKey(p => p.Id);

            builder.Property(p => p.Nome)
            .HasColumnType("VARCHAR")
            .HasMaxLength(50)
            .IsRequired();

            builder.Property(p => p.NomeCurto)
            .HasColumnType("VARCHAR")
            .HasMaxLength(30)
            .IsRequired();

            builder.Property(p => p.TipoPessoa)
            .HasColumnType("VARCHAR")
            .HasMaxLength(15)
            .IsRequired();

            builder.Property(p => p.CpfCnpj)
            .HasColumnType("VARCHAR")
            .HasMaxLength(14)
            .IsRequired();

            builder.Property(p => p.DataCadastro)
            .HasColumnType("timestamp")
            .IsRequired();

            builder.Property(p => p.DataInativacao)
            .HasColumnType("timestamp");
        }
    }
}