using AuthService.Helpers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Compte> Comptes => Set<Compte>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compte>(entity =>
        {
            entity.ToTable("Comptes");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nom).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Prenom).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Telephone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.CIN).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Adresse).HasMaxLength(180).IsRequired();
            entity.Property(x => x.Ville).HasMaxLength(80).IsRequired();
            entity.Property(x => x.CodePostal).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Login).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Password).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(20).IsRequired();
            entity.HasIndex(x => x.Login).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.CIN).IsUnique();

            entity.HasData(
                new Compte
                {
                    Id = 1,
                    Nom = "Admin",
                    Prenom = "System",
                    Email = "admin@livraison.local",
                    Telephone = "70000001",
                    CIN = "ADMIN001",
                    Adresse = "1 Avenue Admin",
                    Ville = "Tunis",
                    CodePostal = "1000",
                    Login = "admin",
                    Password = PasswordHasherHelper.HashPassword("admin123"),
                    Role = "Admin"
                },
                new Compte
                {
                    Id = 2,
                    Nom = "Client",
                    Prenom = "Demo",
                    Email = "user@livraison.local",
                    Telephone = "70000002",
                    CIN = "USER001",
                    Adresse = "2 Avenue Client",
                    Ville = "Sfax",
                    CodePostal = "3000",
                    Login = "user",
                    Password = PasswordHasherHelper.HashPassword("user123"),
                    Role = "User"
                });
        });
    }
}
