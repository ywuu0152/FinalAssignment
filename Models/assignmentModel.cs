namespace FinalAssignment.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class assignmentModel : DbContext
    {
        public assignmentModel()
            : base("name=assignmentModel")
        {
        }

        public virtual DbSet<Lecture> Lectures { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .Property(e => e.locationName)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.latitude)
                .HasPrecision(10, 8);

            modelBuilder.Entity<Location>()
                .Property(e => e.longitude)
                .HasPrecision(11, 8);

            modelBuilder.Entity<Tutor>()
                .Property(e => e.path)
                .IsUnicode(false);
        }
    }

}
