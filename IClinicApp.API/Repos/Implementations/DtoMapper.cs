using System.Security.Claims;
using IClinicApp.API.Dtos.Appointments;
using IClinicApp.API.Dtos.Auth;
using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Filtering;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Dtos.HomePage;
using IClinicApp.API.Dtos.MedicalRecords;
using IClinicApp.API.Dtos.Notifications;
using IClinicApp.API.Dtos.Payments;
using IClinicApp.API.Dtos.Reviews;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Dtos.UserProfile;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Enums;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;

namespace IClinicApp.API.Repos.Implementations
{
    public class DtoMapper(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : IDtoMapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        // Auth Mapping
        public ApplicationUser ToApplicationUser(RegisterDto registerDto)
        {
            return new ApplicationUser
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };
        }
        // Home Page Mapping
        public HomePageDto MapToHomePageDto(string userFullName,IEnumerable<Doctor> doctors, IEnumerable<Specialization> specializations)
        {
            return new HomePageDto
            {
                FullName = userFullName,
                RecommendedDoctors = doctors.Select(d => MapToDoctorDto(d)).ToList(),
                Specializations = specializations.Select(s => MapToSpecializationDto(s)).ToList()
            };
        }

        // Doctor Mapping
        public Doctor MapToAddDoctor(AddDoctorDto addDoctorDto)
        {
            if (addDoctorDto == null) return null!;
            return new Doctor
            {
                FullName = addDoctorDto.FullName,
                ImageUrl = addDoctorDto.ImageUrl,
                Bio = addDoctorDto.Bio,
                SpecializationId = addDoctorDto.SpecializationId, 
                CityId = addDoctorDto.CityId,
                CllinicAddress = addDoctorDto.CllinicAddress,
                PhoneNumber = addDoctorDto.PhoneNumber,
                ClinicName = addDoctorDto.ClinicName,
                ExperienceYears = addDoctorDto.ExperienceYears,
                Price = addDoctorDto.Price,
            };
        }
        public GovernorateDto MapToGovernorateDto(Governorate governorate)
        {
            if (governorate == null) return null!;

            return new GovernorateDto
            {
                Id = governorate.Id,
                Name = governorate.Name,
                Cities = governorate.Cities.Select(c => MapToCityDto(c)).ToList()
            };
        }
        public CityDto MapToCityDto(City city)
        {
            if (city == null) return null!;

            return new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                GovernorateId = city.GovernorateId,
                Doctors = city.Doctors.Select(d => MapToDoctorDto(d)).ToList()
            };
        }
        public SpecializationDto MapToSpecializationDto(Specialization specialization)
        {
            if (specialization == null) return null!;
            return new SpecializationDto
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Doctors = specialization.Doctors.Select(d => MapToDoctorDto(d)).ToList()
            };
        }
        public DoctorDto MapToDoctorDto(Doctor doctor)
        {
            if (doctor == null) return null!;
            return new DoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                ClinicName = doctor.ClinicName,
                SpecializationId = doctor.SpecializationId,
                SpecializationName = doctor.Specialization.Name,
                Rating = doctor.Reviews!.Count != 0 ? doctor.Reviews.Average(r => r.Rating) : 0,
                ReviewsCount = doctor.Reviews.Count,
                ImageUrl = doctor.ImageUrl,
                CityId = doctor.CityId,
                CityName = doctor.City.Name ?? string.Empty,
                GovernorateName = doctor.City.Governorate.Name ?? string.Empty,
            };
        }
        public DoctorProfileDto MapToDoctorProfileDto(Doctor doctor)
        {
            if (doctor == null) return null!;
            return new DoctorProfileDto
            {
                Id = doctor.Id,
                ImageUrl = doctor.ImageUrl,
                FullName = doctor.FullName,
                ClinicName = doctor.ClinicName,
                SpecializationId = doctor.SpecializationId,
                SpecializationName = doctor.Specialization.Name,
                CityId = doctor.CityId,
                CityName = doctor.City.Name,
                GovernorateName = doctor.City.Governorate.Name,
                Rating = doctor.Reviews!.Count != 0 ? doctor.Reviews.Average(r => r.Rating) : 0,
                ReviewsCount = doctor.Reviews.Count,
                Bio = doctor.Bio,
                Price = doctor.Price,
                ClinicAddress = doctor.CllinicAddress,
                PhoneNumber = doctor.PhoneNumber,
                ExperienceYears = doctor.ExperienceYears,
                Reviews = doctor.Reviews?.Select(r => MapToReviewDto(r)).ToList() ?? []
            };
        }

        // Review Mapping
        public ReviewDto MapToReviewDto(Review review)
        {
            if (review == null) return null!;
            return new ReviewDto
            {
                Id = review.Id,
                Comment = review.Comment ?? string.Empty,
                Stars = review.Rating,
                CreatedAt = review.CreatedAt,
                UserFullName = review.Appointment.User.FullName,
                DoctorId = review.DoctorId,
                UserId = review.Appointment.UserId,
            };
        }
        public Review MapToReview(AddReviewDto addReviewDto)
        {
            if (addReviewDto == null) return null!;
            return new Review
            {
                Rating = addReviewDto.Stars,
                Comment = addReviewDto.ReviewText,
                CreatedAt = DateTime.UtcNow,
                AppointmentId = addReviewDto.AppointmentId,
                DoctorId = addReviewDto.DoctorId
            };
        }

        // Payment Mapping
        public PaymentInfoDto MapToPaymentInfoDto(Payment payment)
        {
            if (payment == null) return null!;
            return new PaymentInfoDto
            {
                Id = payment.Id,
                IsConfirmed = payment.IsConfirmed,
                Amount = payment.Amount,
                Method = payment.Method,
                PaymentDate = payment.PaymentDate,
                AppointmentId = payment.AppointmentId
            };
        }
        public Payment MapToPayment(AddPaymentDto addPaymentDto)
        {

            if (addPaymentDto == null) return null!;
            return new Payment
            {
                Id = Guid.NewGuid() ,
                AppointmentId = addPaymentDto.AppointmentId,
                Amount = addPaymentDto.Amount,
                Method = addPaymentDto.Method,
                IsConfirmed = false,
                PaymentDate = DateTime.UtcNow
            };
        }

        // Appointment Mapping
        public AppointmentDetailsDto MapToAppointmentDetailsDto(Appointment appointment)
        {
            if (appointment == null) return null!;

            return new AppointmentDetailsDto
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Status = appointment.Status,
                Doctor = MapToDoctorDto(appointment.Doctor),
                PaymentInfo = appointment.Payment == null ? null : MapToPaymentInfoDto(appointment.Payment)
            };
        }
        public Appointment MapToAppointment(BookAppointmentDto bookAppointmentDto, Guid userId)
        {
            if (bookAppointmentDto == null) return null!;

            return new Appointment
            {
                Id = Guid.NewGuid(), // Generate a new ID for the appointment
                Date = bookAppointmentDto.Date,
                Status = AppointmentStatus.Pending,
                DoctorId = bookAppointmentDto.DoctorId,
                UserId = userId
            };
        }
        public Appointment MapUpadateToAppointment(UpdateAppointmentDto updateAppointmentDto)
        {
            if (updateAppointmentDto == null) return null!;
            return new Appointment
            {
                Id = updateAppointmentDto.Id,
                Date = updateAppointmentDto.Date,
                Status = AppointmentStatus.Pending,
            };
        }

        // User Mapping
        public UserProfileDto MapToUserProfileDto(ApplicationUser? user)
        {
            if (user == null) return null!;
            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!
            };
        }

        // Notification Mapping
        public NotificationDto MapToNotificationDto(Notification notification)
        {
            if (notification == null) return null!;
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead
            };
        }
        // Medical Record Mapping
        public MedicalRecordDto MapToMedicalRecordDto(MedicalRecord medicalRecord)
        {
            if (medicalRecord == null) return null!;
            return new MedicalRecordDto
            {
                Id = medicalRecord.Id,
                UserId = medicalRecord.UserId,
                Title = medicalRecord.Title,
                RecordDate = medicalRecord.RecordDate,
                FileUrl = medicalRecord.FileUrl
            };
        }
    }
}
