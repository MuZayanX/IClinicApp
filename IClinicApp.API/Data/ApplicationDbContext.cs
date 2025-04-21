using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using IClinicApp.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -------------------- Appointment Relationships --------------------

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Review)
                .WithOne(r => r.Appointment)
                .HasForeignKey<Review>(r => r.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------- Review Relationships --------------------

            builder.Entity<Review>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- Doctor Relationships --------------------

            builder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Doctor>()
                .HasOne(d => d.City)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- City and Governorate --------------------

            builder.Entity<City>()
                .HasOne(c => c.Governorate)
                .WithMany(g => g.Cities)
                .HasForeignKey(c => c.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------- MedicalRecord --------------------

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany(u => u.MedicalRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------- Notification --------------------

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------- Optional: Unique Constraints / Indexes --------------------

            builder.Entity<Doctor>()
                .HasIndex(d => new { d.FullName, d.CityId })
                .IsUnique(false);

            builder.Entity<Specialization>()
                .HasIndex(s => s.Name)
                .IsUnique(true);

            builder.Entity<Governorate>()
                .HasIndex(g => g.Name)
                .IsUnique(true);

            builder.Entity<City>()
                .HasIndex(c => new { c.Name, c.GovernorateId })
                .IsUnique(true);

            // -------------------- Optional: Enum to string conversion --------------------

            builder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<string>(); // Enum → string

            builder.Entity<Notification>()
                .Property(n => n.Type)
                .HasConversion<string>(); // Enum → string

            builder.Entity<Payment>()
                .Property(m => m.Method)
                .HasConversion<string>(); // Enum → string
            //---------------------- setting price precision to (10,2)----------------
            builder.Entity<Doctor>()
                .Property(d => d.Price)
                .HasPrecision(10, 2); //Total 10 digits, 2 of them after the decimal point

            // -------------------- Optional: Seed Lookup Tables --------------------

            builder.Entity<Specialization>().HasData(
                new Specialization { Id = Guid.NewGuid(), Name = "Cardiology" },
                new Specialization { Id = Guid.NewGuid(), Name = "Dermatology" },
                new Specialization { Id = Guid.NewGuid(), Name = "Neurology" }
            );

            var cairoId = Guid.NewGuid();
            builder.Entity<Governorate>().HasData(
                new Governorate { Id = cairoId, Name = "Cairo" }
            );

            builder.Entity<City>().HasData(
                new City { Id = Guid.NewGuid(), Name = "Nasr City", GovernorateId = cairoId },
                new City { Id = Guid.NewGuid(), Name = "Heliopolis", GovernorateId = cairoId }
            );
        }
    }
}
