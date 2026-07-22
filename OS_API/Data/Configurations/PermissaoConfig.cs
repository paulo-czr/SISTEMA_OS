using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models;

namespace OS_API.Data.Configurations
{
    public class PermissaoConfig : IEntityTypeConfiguration<PermissaoModel>
    {
        public void Configure(EntityTypeBuilder<PermissaoModel> builder)
        {
            builder.ToTable("Permissao");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Descricao)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Modulo)
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
