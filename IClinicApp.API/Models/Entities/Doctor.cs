using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }

        // Optional link to Identity user (if/when doctor has login)
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;

        public Guid SpecializationId { get; set; }
        public Specialization Specialization { get; set; } = null!;

        public Guid CityId { get; set; }
        public City City { get; set; } = null!;

        public string CllinicAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public int? ExperienceYears { get; set; }
        public decimal? Price { get; set; } // e.g., 100.50 for 100.50 EGP


        public ICollection<Review>? Reviews { get; set; } = [];
        public ICollection<Appointment>? Appointments { get; set; } = [];
    }
}