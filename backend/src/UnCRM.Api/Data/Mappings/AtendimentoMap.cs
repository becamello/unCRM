using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data.Mappings
{
    public class AtendimentoMap : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            builder.ToTable("atendimento")
           .HasKey(p => p.Id);

            builder.HasOne(p => p.TipoAtendimento)
           .WithMany()
           .HasForeignKey(fk => fk.TipoAtendimentoId);

            builder.HasOne(p => p.Pessoa)
            .WithMany()
            .HasForeignKey(fk => fk.PessoaId);

            builder.Property(p => p.UsuarioCriacaoId)
            .HasColumnType("BIGINT")
            .IsRequired();

            builder.Property(p => p.Status)
            .HasColumnType("VARCHAR")
            .HasMaxLength(10)
            .IsRequired();

            builder.HasMany(a => a.Pareceres)
            .WithOne(p => p.Atendimento)
            .HasForeignKey(p => p.AtendimentoId);

            builder.Property(p => p.DataCadastro)
           .HasColumnType("timestamp")
           .IsRequired();

            builder.Property(p => p.DataInativacao)
            .HasColumnType("timestamp");

            builder.OwnsOne(a => a.ProximoContato, proximoContato =>
            {
                proximoContato.Property(p => p.Usuario).HasColumnName("UsuarioProximoContato");
                proximoContato.Property(p => p.Data).HasColumnName("DataProximoContato");
            });
        }
    }
}