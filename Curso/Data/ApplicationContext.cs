using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {

        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p=>p.AddConsole());
       
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer(connectionString: @"Data Source=DESKTOP-DS61A0Q\SQLEXPRESS;Initial Catalog=tb_cursoEFCore;Integrated Security=True", 
                 p=>p.EnableRetryOnFailure(
                     maxRetryCount: 2, 
                     maxRetryDelay:TimeSpan.FromSeconds(5),
                     errorNumbersToAdd: null).MigrationsHistoryTable("Curso_EFCore")
                 );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //...
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        property.SetMaxLength(100);//OU
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }

    }
}
