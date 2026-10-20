using chdScoring.DataAccess.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace chdScoring.DataAccess.EFCore
{
    public class chdScoringContext(DbContextOptions<chdScoringContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKey>(builder =>
            {
                builder.ToTable("apikey").HasKey(x => x.Id);

                builder.Property(p => p.Redirect).IsRequired(false);
            });

            modelBuilder.Entity<Round>(builder =>
            {
                builder.ToTable("durchgang").HasKey(x => new { x.Teilnehmer, x.Durchgang });
            });
            modelBuilder.Entity<Durchgang_Panel>().HasKey(x => new { x.Panel, x.Durchgang });
            modelBuilder.Entity<Durchgang_Programm>(builder =>
            {
                builder.ToTable("durchgang_programm").HasKey(x => new { x.Programm, x.Durchgang });
            });
            modelBuilder.Entity<Figur_Programm>().HasKey(x => new { x.Programm, x.Figur });
            modelBuilder.Entity<Teilnehmer_Bewerb>().HasKey(x => new { x.Bewerb, x.Teilnehmer });
            modelBuilder.Entity<Judge_Panel>().HasKey(x => new { x.Judge, x.Panel });
            modelBuilder.Entity<Wertung_History>().HasKey(x => new { x.Teilnehmer, x.Durchgang, x.Figur, x.Judge, x.Time });
            modelBuilder.Entity<Teilnehmer_Durchgang_Judge>(builder =>
            {
                builder.ToTable("teilnehmer_durchgang_judge");
                builder.HasKey(x => new { x.Teilnehmer, x.Durchgang, x.Judge });
            });
            modelBuilder.Entity<Wertung>(builder =>
            {
                builder.ToTable("wertung").HasKey(x => new { x.Teilnehmer, x.Durchgang, x.Figur, x.Judge });
                builder.HasMany(m => m.Histories).WithOne(o => o.Wertung).HasForeignKey(x => new { x.Teilnehmer, x.Durchgang, x.Figur, x.Judge });
            });

            modelBuilder.Entity<Country_Images>().HasKey(x => x.Img_Id);
            modelBuilder.Entity<Images>().HasKey(x => x.Img_Id);
            modelBuilder.Entity<Klassen>().HasKey(x => x.Id);
            modelBuilder.Entity<Teilnehmer>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Country_Image).WithMany().HasForeignKey(f => f.Land);
                builder.HasOne(x => x.Image).WithMany().HasForeignKey(f => f.Bild);
            });
            modelBuilder.Entity<Wettkampf_Leitung>(builder =>
            {
                builder.ToTable("wettkampf_leitung").HasKey(x => new { x.Teilnehmer, x.Durchgang });
                builder.HasOne(x => x.Pilot).WithMany().HasForeignKey(f => f.Teilnehmer);
                builder.HasOne(x => x.Round).WithMany().HasForeignKey(f => new { f.Teilnehmer, f.Durchgang });
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}