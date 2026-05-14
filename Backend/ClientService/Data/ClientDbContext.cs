using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions<ClientDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Email).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Telephone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.CIN).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Adresse).HasMaxLength(180).IsRequired();
            entity.Property(x => x.Nom).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Prenom).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Ville).HasMaxLength(80).IsRequired();
            entity.Property(x => x.CodePostal).HasMaxLength(10).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.CIN).IsUnique();
            entity.HasIndex(x => x.CompteId).IsUnique().HasFilter("[CompteId] IS NOT NULL");

            entity.HasData(
                new Client
                {
                    Id = 1, Nom = "Ben Salah", Prenom = "Amine", Email = "amine.client@livraison.local",
                    Telephone = "71111111", CIN = "CL001", Adresse = "10 Rue de Marseille", Ville = "Tunis", CodePostal = "1000"
                },
                new Client
                {
                    Id = 2, Nom = "Trabelsi", Prenom = "Sarra", Email = "sarra.client@livraison.local",
                    Telephone = "72222222", CIN = "CL002", Adresse = "25 Avenue Habib Bourguiba", Ville = "Sfax", CodePostal = "3000"
                },
                new Client
                {
                    Id = 3, Nom = "Jaziri", Prenom = "Youssef", Email = "youssef.client@livraison.local",
                    Telephone = "73333333", CIN = "CL003", Adresse = "8 Rue des Oliviers", Ville = "Sousse", CodePostal = "4000"
                });
        });
    }
}
