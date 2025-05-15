using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data.Mappings
{
    public class TipoAtendimentoMap : IEntityTypeConfiguration<TipoAtendimento>
    {
        public void Configure(EntityTypeBuilder<TipoAtendimento> builder)
        {
            builder.ToTable("tipo_atendimento")
           .HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

            builder.HasData(
             new TipoAtendimento { Id = 1, Descricao = "Renovação de Contrato" },
             new TipoAtendimento { Id = 2, Descricao = "Atualização de Sistema" },
             new TipoAtendimento { Id = 3, Descricao = "Dúvida sobre o sistema" },
             new TipoAtendimento { Id = 4, Descricao = "Erro de Sistema" },
             new TipoAtendimento { Id = 5, Descricao = "Reclamação" },
             new TipoAtendimento { Id = 6, Descricao = "Cancelamento" });

        }
    }
}