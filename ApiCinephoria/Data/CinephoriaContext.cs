using Microsoft.EntityFrameworkCore;
using ApiCinephoria.Models;

namespace ApiCinephoria.Data
{
    public class CinephoriaContext(DbContextOptions<CinephoriaContext> options) : DbContext(options)
    {
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<CinemaModel> Cinemas { get; set; }
        public DbSet<CinemaScheduleModel> Cinema_schedules { get; set; }
        public DbSet<MovieTimesModel> MovieTimes { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<SeatsModel> Locations { get; set; }
        public DbSet<LoginCreateModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<IncidentModel> Incidents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IncidentModel>()
                .ToTable("incident")
                .HasKey(s => s.IncidentId);

            modelBuilder.Entity<IncidentModel>()
                .HasOne<IncidentModel>()
                .WithMany()
                .HasForeignKey(s => s.CinemaId);

            modelBuilder.Entity<IncidentModel>()
                .HasOne<IncidentModel>()
                .WithMany()
                .HasForeignKey(s => s.RoomId);

            modelBuilder.Entity<IncidentModel>()
                .HasOne<IncidentModel>()
                .WithMany()
                .HasForeignKey(s => s.LocationId);

            modelBuilder.Entity<CinemaScheduleModel>()
                .ToTable("cinema_schedule")
                .HasKey(s => s.ScheduleId);

            modelBuilder.Entity<CinemaScheduleModel>()
                .HasOne<CinemaModel>()
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.CinemaId);

            modelBuilder.Entity<MovieTimesModel>()
                .ToTable("movietimes")
                .HasKey(mt => mt.MovieTimesId);

            modelBuilder.Entity<MovieTimesModel>()
                .HasOne(mt => mt.Movie)
                .WithMany()
                .HasForeignKey(mt => mt.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieTimesModel>()
                .HasOne(mt => mt.Cinema)
                .WithMany()
                .HasForeignKey(mt => mt.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieTimesModel>()
                .HasOne(mt => mt.Room)
                .WithMany()
                .HasForeignKey(mt => mt.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomModel>()
                .ToTable("rooms")
                .HasKey(mt => mt.RoomId);

            modelBuilder.Entity<RoomModel>()
                .HasMany(r => r.Seats)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeatsModel>()
                .ToTable("locations")
                .HasKey(mt => mt.LocationId);

            modelBuilder.Entity<SeatsModel>()
                .HasOne(mt => mt.Room)
                .WithMany(r => r.Seats)
                .HasForeignKey(mt => mt.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoginCreateModel>()
                .ToTable("users")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<LoginCreateModel>()
                .HasOne(u => u.Role)        // Navigation vers RoleModel
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                       



        }
    }
}
