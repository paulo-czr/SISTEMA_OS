using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OS_API.Models.Cliente;

namespace OS_API.Data.Configurations
{
    public class ClienteConfig : IEntityTypeConfiguration<ClienteModel>
    {
        public void Configure(EntityTypeBuilder<ClienteModel> builder)
        {
            builder.HasKey(c => c.IdCliente);

            builder.Property(c => c.RazaoSocial)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(c => c.NomeFantasia)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.TipoPessoa)
                   .IsRequired();

            builder.Property(c => c.Documento)
                   .IsRequired()
                   .HasMaxLength(14); // CNPJ (14 dígitos) é o maior dos dois documentos possíveis

            // A verificação de duplicidade feita no ClienteService evita a maioria dos casos,
            // mas não protege contra requisições concorrentes (race condition). O índice único
            // garante a integridade dos dados também no nível do banco.
            builder.HasIndex(c => c.Documento)
                   .IsUnique();

            builder.Property(c => c.Telefone)
                   .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .HasMaxLength(254);

            builder.Property(c => c.Cep)
                   .IsRequired()
                   .HasMaxLength(8);

            builder.Property(c => c.Uf)
                   .HasMaxLength(2);

            builder.Property(c => c.Cidade)
                   .HasMaxLength(100);

            builder.Property(c => c.Rua)
                   .HasMaxLength(200);

            builder.Property(c => c.Numero)
                   .HasMaxLength(20);

            builder.Property(c => c.Ativo)
                   .IsRequired();
        }
    }
}
