using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models;

namespace OS_API.Data.Configurations
{
    public class OrdemServicoConfig : IEntityTypeConfiguration<OrdemServicoModel>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoModel> builder)
        {
            builder.HasKey(o => o.IdOs);

            builder.Property(o => o.TituloOs)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(o => o.Descricao)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired();

            builder.HasOne(o => o.Cliente)
                   .WithMany()
                   .HasForeignKey(o => o.IdCliente)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.UsuarioQueRegistrou)
                   .WithMany()
                   .HasForeignKey(o => o.IdUsuarioQueRegistrou)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.TipoAtendimento)
                   .WithMany(t => t.OrdensServico)
                   .HasForeignKey(o => o.IdTipoAtendimento)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Funcionarios)
                   .WithOne(f => f.OrdemServico)
                   .HasForeignKey(f => f.IdOs)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
