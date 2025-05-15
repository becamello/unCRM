using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data.Mappings
{
    public class ParecerMap : IEntityTypeConfiguration<Parecer>
    {
        public void Configure(EntityTypeBuilder<Parecer> builder)
        {
            builder.ToTable("parecer")
           .HasKey(p => p.Id);

            builder.Property(p => p.AtendimentoId)
            .HasColumnType("BIGINT")
            .IsRequired();

            builder.Property(p => p.UsuarioCriacaoId)
            .HasColumnType("BIGINT")
            .IsRequired();

            builder.Property(p => p.Descricao)
            .HasColumnType("VARCHAR")
            .HasMaxLength(500)
            .IsRequired();

            builder.Property(p => p.Data)
            .HasColumnType("timestamp")
            .IsRequired();
        }
    }
}