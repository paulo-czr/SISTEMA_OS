using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models;

namespace OS_API.Data.Configurations
{
    public class AssinaturaOsConfiguration : IEntityTypeConfiguration<AssinaturaOsModel>
    {
        public void Configure(EntityTypeBuilder<AssinaturaOsModel> builder)
        {
            builder.ToTable("Assinaturas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NomeSignatario)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.DocumentoSignatario)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.ImagemAssinatura)
                .IsRequired();

            builder.Property(x => x.DataAssinatura)
                .IsRequired();

            builder.Property(x => x.Ip)
                .HasMaxLength(50);

            builder.Property(x => x.UserAgente)
                .HasMaxLength(500);

            builder.Property(x => x.Tipo)
                .HasConversion<int>();

            builder.HasOne(x => x.OrdemServico)
                .WithMany(x => x.Assinaturas)
                .HasForeignKey(x => x.IdOs)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}