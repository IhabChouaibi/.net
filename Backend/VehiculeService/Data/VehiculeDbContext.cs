using Microsoft.EntityFrameworkCore;
using VehiculeService.Models;

namespace VehiculeService.Data;

public class VehiculeDbContext : DbContext
{
    public VehiculeDbContext(DbContextOptions<VehiculeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicule> Vehicules => Set<Vehicule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicule>(entity =>
        {
            entity.ToTable("Vehicules");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Couleur).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Marque).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Matricule).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Type).HasMaxLength(20).IsRequired();
            entity.HasIndex(x => x.Matricule).IsUnique();

            entity.HasDiscriminator(x => x.Type)
                .HasValue<Vehicule>("Vehicule")
                .HasValue<Camion>("Camion")
                .HasValue<Voiture>("Voiture");
        });

        modelBuilder.Entity<Camion>().HasData(
            new Camion
            {
                Id = 1,
                Type = "Camion",
                Couleur = "Blanc",
                Marque = "Volvo",
                Matricule = "TU-1234",
                VitesseLimite = 90,
                Capacite = 12000,
                NbrEssieux = 3
            });

        modelBuilder.Entity<Voiture>().HasData(
            new Voiture
            {
                Id = 2,
                Type = "Voiture",
                Couleur = "Gris",
                Marque = "Peugeot",
                Matricule = "TU-5678",
                VitesseLimite = 120,
                NbrPlaces = 5
            });
    }
}
