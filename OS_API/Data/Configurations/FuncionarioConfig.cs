using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models;

namespace OS_API.Data.Configurations
{
    public class FuncionarioConfig : IEntityTypeConfiguration<FuncionarioModel>
    {
        public void Configure(EntityTypeBuilder<FuncionarioModel> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(e => e.Usuario)
              .WithOne(u => u.Funcionario)
              .HasForeignKey<FuncionarioModel>(e => e.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
