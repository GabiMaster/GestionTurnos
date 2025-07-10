using GestionTurnos.BackEnd.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnos.BackEnd.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Turno> Turnos => Set<Turno>();
        public DbSet<Servicio> Servicios => Set<Servicio>();
        public DbSet<Profesional> Profesionales => Set<Profesional>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();

            // Relación N:N Profesional-Servicio
            modelBuilder.Entity<Profesional>()
                .HasMany(p => p.Servicios)
                .WithMany();
        }
    }
}
