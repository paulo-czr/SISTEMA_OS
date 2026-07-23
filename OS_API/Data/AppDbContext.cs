using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OS_API.Models;
using OS_API.Models.Cliente;

namespace OS_API.Data
{
    public class AppDbContext : IdentityDbContext<UsuarioModel>
    {

        // Construtor utilizado pelo Entity Framework para receber
        // as configurações de conexão registradas no Program.cs.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Cada DbSet representa uma tabela do banco.
        public DbSet<FuncionarioModel> Funcionarios { get; set; }
        public DbSet<PermissaoModel> Permissoes { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<OrdemServicoModel> OrdensServico { get; set; }
        public DbSet<OsFuncionarioModel> OsFuncionarios { get; set; }
        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioModel>().ToTable("Usuarios");

            modelBuilder.Entity<IdentityRole>().ToTable("Perfis");

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuarioPerfis");

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuarioClaims");

            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuarioLogins");

            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("PerfilClaims");

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuarioTokens");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}