using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
            .HasKey(p => p.Id);

            builder.Property(p => p.Login)
            .HasColumnType("VARCHAR")
            .IsRequired();

            builder.Property(p => p.Senha)
            .HasColumnType("VARCHAR")
            .IsRequired();
            
            builder.Property(p => p.Nome)
            .HasColumnType("VARCHAR")
            .IsRequired();

            builder.Property(p => p.Cargo)
            .HasColumnType("VARCHAR")
            .IsRequired();

            builder.Property(p => p.DataCadastro)
            .HasColumnType("timestamp")
            .IsRequired();

            builder.Property(p => p.DataInativacao)
            .HasColumnType("timestamp");
        }
    }
}