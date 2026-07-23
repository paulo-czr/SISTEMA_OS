using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models;

namespace OS_API.Data.Configurations
{
    public class OsFuncionarioConfig : IEntityTypeConfiguration<OsFuncionarioModel>
    {
        public void Configure(EntityTypeBuilder<OsFuncionarioModel> builder)
        {
            builder.HasKey(of => of.IdOsFuncionario);

            builder.HasOne(of => of.OrdemServico)
                   .WithMany(o => o.Funcionarios)
                   .HasForeignKey(of => of.IdOs)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(of => of.funcionario)
                   .WithMany()
                   .HasForeignKey(of => of.IdFuncionario)
                   .OnDelete(DeleteBehavior.Restrict);

            // Um mesmo funcionário não deve ser vinculado duas vezes à mesma OS.
            builder.HasIndex(of => new { of.IdOs, of.IdFuncionario })
                   .IsUnique();
        }
    }
}
