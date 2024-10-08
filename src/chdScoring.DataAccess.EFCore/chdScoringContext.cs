using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace chdScoring.DataAccess.EFCore
{
    public class chdScoringContext : DbContext
    {

        public DbSet<Teilnehmer> Teilnehmer { get; set; }
        public DbSet<Stammdaten> Stammdaten { get; set; }
        public DbSet<Durchgang_Panel> Durchgang_Panel { get; set; }
        public DbSet<Figur> Figur { get; set; }
        public DbSet<Figur_Programm> Figur_Programm { get; set; }
        public DbSet<Round> Durchgang { get; set; }
        public DbSet<Programm> Programm { get; set; }
        public DbSet<Durchgang_Programm> Durchgang_Programm { get; set; }
        public DbSet<Wertung> Wertung { get; set; }
        public DbSet<Judge> Judge { get; set; }
        public DbSet<Judge_Panel> Judge_Panel { get; set; }
        public DbSet<Country_Images> Country_Images { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Wettkampf_Leitung> Wettkampf_Leitung { get; set; }
        public DbSet<Klassen> Klassen { get; set; }


        public chdScoringContext(DbContextOptions<chdScoringContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Round>(builder =>
            {
                builder.ToTable("durchgang").HasKey(x => x.Durchgang);
            });
            modelBuilder.Entity<Durchgang_Panel>().HasKey(x => new { x.Panel, x.Durchgang });
            modelBuilder.Entity<Durchgang_Programm>().HasKey(x => new { x.Programm, x.Durchgang });
            modelBuilder.Entity<Figur_Programm>().HasKey(x => new { x.Programm, x.Figur });
            modelBuilder.Entity<Judge_Panel>().HasKey(x => new { x.Judge, x.Panel });
            modelBuilder.Entity<Wertung>().HasKey(x => new { x.Judge, x.Durchgang, x.Figur, x.Teilnehmer });
            modelBuilder.Entity<Country_Images>().HasKey(x => x.Img_Id);
            modelBuilder.Entity<Images>().HasKey(x => x.Img_Id);
            modelBuilder.Entity<Klassen>().HasKey(x => x.Id);
            modelBuilder.Entity<Teilnehmer>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Country_Image).WithOne().HasForeignKey<Teilnehmer>(f => f.Land);
                builder.HasOne(x => x.Image).WithOne().HasForeignKey<Teilnehmer>(f => f.Bild);
            });
            modelBuilder.Entity<Wettkampf_Leitung>(builder =>
            {
                builder.ToTable("wettkampf_leitung").HasKey(x => new { x.Teilnehmer, x.Durchgang });
                builder.HasOne(x => x.Pilot).WithOne().HasForeignKey<Wettkampf_Leitung>(f => f.Teilnehmer);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}