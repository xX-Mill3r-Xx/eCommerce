using Microsoft.EntityFrameworkCore;

namespace eCommerce.API.Database
{
    public class eCommerceContext : DbContext
    {
        public eCommerceContext(DbContextOptions<eCommerceContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<EnderecoEntrega> EnderecosEntrega { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Como criar carga seed

            /*
                modelBuilder.Entity<Departamento>().HasData(
                    new Departamento { Id = 2, Nome = "Moda" },
                    new Departamento { Id = 3, Nome = "Informática" },
                    new Departamento { Id = 4, Nome = "Eletrodomésticos" },
                    new Departamento { Id = 5, Nome = "Eletroportateis" },
                    new Departamento { Id = 6, Nome = "Cama/Mesa/Banho" }
                );
             */

            //após isso, rodar a migration e update database.

            #endregion
        }
    }
}
