using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Models;

namespace ProfessionalsSiancaValley.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Miniature> Miniatures => Set<Miniature>();
        public DbSet<Report> Reports => Set<Report>();

        // ===================================================
        // GENERADOR AUTOMÁTICO DEL ID_USER
        // ===================================================
        private static string GenerarIdUser(
            int posicion,
            string firstName,
            string lastName)
        {
            // posición normal con cero delante
            string posNormal = posicion.ToString("D2");

            // posición invertida
            string posInvertida = new string(posNormal.Reverse().ToArray());

            // iniciales
            string iniciales =
                $"{firstName.FirstOrDefault()}{lastName.FirstOrDefault()}"
                .ToUpper();

            // resultado final
            return $"{posNormal}{iniciales}{posInvertida}";
        }

        // ===================================================
        // OVERRIDE SAVECHANGES (AUTOMÁTICO)
        // ===================================================
        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            var nuevosUsuarios = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .ToList();

            // ================================
            // FECHAS AUTOMÁTICAS
            // ================================
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<User>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }

            // ================================
            // GENERAR ID_USER
            // ================================
            await Database.OpenConnectionAsync(cancellationToken);

            try
            {
                foreach (var user in nuevosUsuarios)
                {
                    if (string.IsNullOrEmpty(user.IdUser))
                    {
                        await using var cmd =
                            Database.GetDbConnection().CreateCommand();

                        cmd.CommandText =
                            "SELECT nextval(pg_get_serial_sequence('users','user_position'))";

                        var resultPos =
                            await cmd.ExecuteScalarAsync(cancellationToken);

                        int nextPosition = Convert.ToInt32(resultPos);

                        user.UserPosition = nextPosition;

                        user.IdUser = GenerarIdUser(
                            nextPosition,
                            user.FirstName,
                            user.LastName);
                    }
                }
            }
            finally
            {
                await Database.CloseConnectionAsync();
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Miniature)
                .WithMany()
                .HasForeignKey(r => r.Id_Miniature)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}


