﻿using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Patients;
using IClinicApp.API.Dtos.Payments;
using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Appointments
{
    public class AppointmentDetailsDto
    {
        public Guid Id { get; set; }
        public DoctorDto Doctor { get; set; } = null!;
        public PatientInfoDto Patient { get; set; } = null!; 
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }

        public PaymentInfoDto? PaymentInfo { get; set; } // ✅ Optional, clean structure
    }
}
