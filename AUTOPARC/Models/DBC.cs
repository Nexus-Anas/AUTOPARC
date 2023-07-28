using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class DBC : DbContext
    {
        public DBC()
        {
        }

        public DBC(DbContextOptions<DBC> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Chauffeurs> Chauffeurs { get; set; }
        public virtual DbSet<Docs> Docs { get; set; }
        public virtual DbSet<EtatVehicules> EtatVehicules { get; set; }
        public virtual DbSet<Fournisseurs> Fournisseurs { get; set; }
        public virtual DbSet<HistoriqueChauffeurVehicule> HistoriqueChauffeurVehicule { get; set; }
        public virtual DbSet<Maintenances> Maintenances { get; set; }
        public virtual DbSet<Marques> Marques { get; set; }
        public virtual DbSet<MethodePayements> MethodePayements { get; set; }
        public virtual DbSet<Modeles> Modeles { get; set; }
        public virtual DbSet<RechargeCarburants> RechargeCarburants { get; set; }
        public virtual DbSet<TypeCarburants> TypeCarburants { get; set; }
        public virtual DbSet<TypeDocs> TypeDocs { get; set; }
        public virtual DbSet<TypeFournisseurs> TypeFournisseurs { get; set; }
        public virtual DbSet<TypeMaintenances> TypeMaintenances { get; set; }
        public virtual DbSet<Vehicules> Vehicules { get; set; }
        public virtual DbSet<Ventes> Ventes { get; set; }
        public virtual DbSet<Villes> Villes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=autoparc_db;uid=root", x => x.ServerVersion("10.4.27-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("categories");

                entity.HasIndex(e => e.Categorie)
                    .HasName("Categorie")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Categorie)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Chauffeurs>(entity =>
            {
                entity.ToTable("chauffeurs");

                entity.HasIndex(e => e.Email)
                    .HasName("Email")
                    .IsUnique();

                entity.HasIndex(e => e.Portable)
                    .HasName("Portable")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Adresse)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.DateEmbauche).HasColumnType("date");

                entity.Property(e => e.DateExpirationPermis).HasColumnType("date");

                entity.Property(e => e.DateNaissance)
                    .HasColumnName("Date_Naissance")
                    .HasColumnType("date");

                entity.Property(e => e.Disponibilite)
                    .HasColumnType("varchar(3)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.NumeroPermisConduire)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Portable)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Remarques)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Docs>(entity =>
            {
                entity.ToTable("docs");

                entity.HasIndex(e => e.FrsId)
                    .HasName("FK_DOC_FRS");

                entity.HasIndex(e => e.TypeId)
                    .HasName("FK_DOC_TYPEDOC");

                entity.HasIndex(e => e.VehiculeId)
                    .HasName("FK_DOC_VEHICULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateDebut)
                    .HasColumnName("Date_Debut")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateFin)
                    .HasColumnName("Date_Fin")
                    .HasColumnType("datetime");

                entity.Property(e => e.FrsId)
                    .HasColumnName("Frs_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.TypeId)
                    .HasColumnName("Type_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UrlDoc)
                    .HasColumnName("Url_Doc")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VehiculeId)
                    .HasColumnName("Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Frs)
                    .WithMany(p => p.Docs)
                    .HasForeignKey(d => d.FrsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOC_FRS");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Docs)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOC_TYPEDOC");

                entity.HasOne(d => d.Vehicule)
                    .WithMany(p => p.Docs)
                    .HasForeignKey(d => d.VehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOC_VEHICULE");
            });

            modelBuilder.Entity<EtatVehicules>(entity =>
            {
                entity.ToTable("etat_vehicules");

                entity.HasIndex(e => e.Etat)
                    .HasName("Etat")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Etat)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Fournisseurs>(entity =>
            {
                entity.ToTable("fournisseurs");

                entity.HasIndex(e => e.Email)
                    .HasName("Email")
                    .IsUnique();

                entity.HasIndex(e => e.Nom)
                    .HasName("Nom")
                    .IsUnique();

                entity.HasIndex(e => e.Portable)
                    .HasName("Portable")
                    .IsUnique();

                entity.HasIndex(e => e.Telephone)
                    .HasName("Telephone")
                    .IsUnique();

                entity.HasIndex(e => e.TypeFrsId)
                    .HasName("FK_FRS_TYPEFRS");

                entity.HasIndex(e => e.VilleId)
                    .HasName("FK_FRS_VILLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Adresse)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Portable)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Telephone)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.TypeFrsId)
                    .HasColumnName("TypeFrs_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VilleId)
                    .HasColumnName("Ville_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.TypeFrs)
                    .WithMany(p => p.Fournisseurs)
                    .HasForeignKey(d => d.TypeFrsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FRS_TYPEFRS");

                entity.HasOne(d => d.Ville)
                    .WithMany(p => p.Fournisseurs)
                    .HasForeignKey(d => d.VilleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FRS_VILLE");
            });

            modelBuilder.Entity<HistoriqueChauffeurVehicule>(entity =>
            {
                entity.ToTable("historique_chauffeur_vehicule");

                entity.HasIndex(e => e.ChauffeurId)
                    .HasName("FK_HISTORIQUE_CHAUFFEUR");

                entity.HasIndex(e => e.VehiculeId)
                    .HasName("FK_HISTORIQUE_VEHICULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ChauffeurId)
                    .HasColumnName("Chauffeur_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateDebutAffectation).HasColumnType("date");

                entity.Property(e => e.DateFinAffectation).HasColumnType("date");

                entity.Property(e => e.RaisonFinAffectation)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VehiculeId)
                    .HasColumnName("Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Chauffeur)
                    .WithMany(p => p.HistoriqueChauffeurVehicule)
                    .HasForeignKey(d => d.ChauffeurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HISTORIQUE_CHAUFFEUR");

                entity.HasOne(d => d.Vehicule)
                    .WithMany(p => p.HistoriqueChauffeurVehicule)
                    .HasForeignKey(d => d.VehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HISTORIQUE_VEHICULE");
            });

            modelBuilder.Entity<Maintenances>(entity =>
            {
                entity.ToTable("maintenances");

                entity.HasIndex(e => e.MethodePayementId)
                    .HasName("FK_MAINTENANCE_PAYEMENT");

                entity.HasIndex(e => e.TypeId)
                    .HasName("FK_MAINTENANCE_TYPEMAINTENANCE");

                entity.HasIndex(e => e.VehiculeId)
                    .HasName("FK_MAINTENANCE_VEHICULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cout).HasColumnType("decimal(9,2)");

                entity.Property(e => e.DateMaintenance)
                    .HasColumnName("Date_Maintenance")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MethodePayementId)
                    .HasColumnName("Methode_Payement_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MontantPayee)
                    .HasColumnName("Montant_Payee")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.TypeId)
                    .HasColumnName("Type_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UrlDoc)
                    .HasColumnName("Url_Doc")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VehiculeId)
                    .HasColumnName("Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.MethodePayement)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.MethodePayementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAINTENANCE_PAYEMENT");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAINTENANCE_TYPEMAINTENANCE");

                entity.HasOne(d => d.Vehicule)
                    .WithMany(p => p.Maintenances)
                    .HasForeignKey(d => d.VehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAINTENANCE_VEHICULE");
            });

            modelBuilder.Entity<Marques>(entity =>
            {
                entity.ToTable("marques");

                entity.HasIndex(e => e.Marque)
                    .HasName("Marque")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Marque)
                    .IsRequired()
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<MethodePayements>(entity =>
            {
                entity.ToTable("methode_payements");

                entity.HasIndex(e => e.Methode)
                    .HasName("Methode")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Methode)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Modeles>(entity =>
            {
                entity.ToTable("modeles");

                entity.HasIndex(e => e.MarqueId)
                    .HasName("FK_MODELE_MARQUE");

                entity.HasIndex(e => e.Modele)
                    .HasName("Modele")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MarqueId)
                    .HasColumnName("Marque_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Modele)
                    .IsRequired()
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.Marque)
                    .WithMany(p => p.Modeles)
                    .HasForeignKey(d => d.MarqueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MODELE_MARQUE");
            });

            modelBuilder.Entity<RechargeCarburants>(entity =>
            {
                entity.ToTable("recharge_carburants");

                entity.HasIndex(e => e.MethodePayementId)
                    .HasName("FK_RECHARGE_PAYEMENT");

                entity.HasIndex(e => e.VehiculeId)
                    .HasName("FK_RECHARGE_VEHICULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateRecharge)
                    .HasColumnName("Date_Recharge")
                    .HasColumnType("datetime");

                entity.Property(e => e.Km).HasColumnType("decimal(9,2)");

                entity.Property(e => e.MethodePayementId)
                    .HasColumnName("Methode_Payement_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Note)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Pu).HasColumnType("decimal(9,2)");

                entity.Property(e => e.Quantite).HasColumnType("decimal(6,2)");

                entity.Property(e => e.VehiculeId)
                    .HasColumnName("Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.MethodePayement)
                    .WithMany(p => p.RechargeCarburants)
                    .HasForeignKey(d => d.MethodePayementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RECHARGE_PAYEMENT");

                entity.HasOne(d => d.Vehicule)
                    .WithMany(p => p.RechargeCarburants)
                    .HasForeignKey(d => d.VehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RECHARGE_VEHICULE");
            });

            modelBuilder.Entity<TypeCarburants>(entity =>
            {
                entity.ToTable("type_carburants");

                entity.HasIndex(e => e.Type)
                    .HasName("Type")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<TypeDocs>(entity =>
            {
                entity.ToTable("type_docs");

                entity.HasIndex(e => e.Type)
                    .HasName("Type")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<TypeFournisseurs>(entity =>
            {
                entity.ToTable("type_fournisseurs");

                entity.HasIndex(e => e.Type)
                    .HasName("Type")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<TypeMaintenances>(entity =>
            {
                entity.ToTable("type_maintenances");

                entity.HasIndex(e => e.Type)
                    .HasName("Type")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Vehicules>(entity =>
            {
                entity.ToTable("vehicules");

                entity.HasIndex(e => e.CarburantId)
                    .HasName("FK_VEHICULE_CARBURANT");

                entity.HasIndex(e => e.CategorieId)
                    .HasName("FK_VEHICULE_CATEGORIE");

                entity.HasIndex(e => e.EtatVehiculeId)
                    .HasName("FK_VEHICULE_ETAT");

                entity.HasIndex(e => e.FrsId)
                    .HasName("FK_VEHICULE_FRS");

                entity.HasIndex(e => e.MarqueId)
                    .HasName("FK_VEHICULE_MARQUE");

                entity.HasIndex(e => e.Matricule)
                    .HasName("Matricule")
                    .IsUnique();

                entity.HasIndex(e => e.MethodePayementId)
                    .HasName("FK_VEHICULE_PAYEMENT");

                entity.HasIndex(e => e.ModeleId)
                    .HasName("FK_VEHICULE_MODELE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Architecture)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.BoiteAVitesse)
                    .HasColumnName("Boite_A_Vitesse")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.CarburantId)
                    .HasColumnName("Carburant_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategorieId)
                    .HasColumnName("Categorie_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ConsomationMixte)
                    .HasColumnName("Consomation_Mixte")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.DateAchat)
                    .HasColumnName("Date_Achat")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateMisEnCirculation)
                    .HasColumnName("Date_Mis_En_Circulation")
                    .HasColumnType("datetime");

                entity.Property(e => e.EtatVehiculeId)
                    .HasColumnName("Etat_Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FrsId)
                    .HasColumnName("Frs_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Hauteur)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Largeur)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Longueur)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MarqueId)
                    .HasColumnName("Marque_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Matricule)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.MethodePayementId)
                    .HasColumnName("Methode_Payement_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModeleId)
                    .HasColumnName("Modele_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MontantPayee)
                    .HasColumnName("Montant_Payee")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.Moteur)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.NbrPlace)
                    .HasColumnName("Nbr_Place")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Note)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Poids)
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PrixAchat)
                    .HasColumnName("Prix_Achat")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.PuissanceFiscale)
                    .HasColumnName("Puissance_Fiscale")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PuissanceMax)
                    .HasColumnName("Puissance_Max")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VitesseMax)
                    .HasColumnName("Vitesse_Max")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.VolumeCoffre)
                    .HasColumnName("Volume_Coffre")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.HasOne(d => d.Carburant)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.CarburantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_CARBURANT");

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_CATEGORIE");

                entity.HasOne(d => d.EtatVehicule)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.EtatVehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_ETAT");

                entity.HasOne(d => d.Frs)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.FrsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_FRS");

                entity.HasOne(d => d.Marque)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.MarqueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_MARQUE");

                entity.HasOne(d => d.MethodePayement)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.MethodePayementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_PAYEMENT");

                entity.HasOne(d => d.Modele)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.ModeleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VEHICULE_MODELE");
            });

            modelBuilder.Entity<Ventes>(entity =>
            {
                entity.ToTable("ventes");

                entity.HasIndex(e => e.MethodePayementId)
                    .HasName("FK_VENTE_PAYEMENT");

                entity.HasIndex(e => e.VehiculeId)
                    .HasName("FK_VENTE_VEHICULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContactAcheteur)
                    .HasColumnName("Contact_Acheteur")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.DateVente)
                    .HasColumnName("Date_Vente")
                    .HasColumnType("datetime");

                entity.Property(e => e.MethodePayementId)
                    .HasColumnName("Methode_Payement_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MontantRecu)
                    .HasColumnName("Montant_Recu")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.NomAcheteur)
                    .HasColumnName("Nom_Acheteur")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PrixVente)
                    .HasColumnName("Prix_Vente")
                    .HasColumnType("decimal(9,2)");

                entity.Property(e => e.VehiculeId)
                    .HasColumnName("Vehicule_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.MethodePayement)
                    .WithMany(p => p.Ventes)
                    .HasForeignKey(d => d.MethodePayementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VENTE_PAYEMENT");

                entity.HasOne(d => d.Vehicule)
                    .WithMany(p => p.Ventes)
                    .HasForeignKey(d => d.VehiculeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VENTE_VEHICULE");
            });

            modelBuilder.Entity<Villes>(entity =>
            {
                entity.ToTable("villes");

                entity.HasIndex(e => e.Nom)
                    .HasName("Nom")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
