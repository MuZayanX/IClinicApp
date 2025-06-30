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

namespace IClinicApp.API.Repos.Services
{
    public interface IDtoMapper
    {
        ApplicationUser ToApplicationUser(RegisterDto registerDto);
        Doctor MapToAddDoctor(AddDoctorDto addDoctorDto);
        GovernorateDto MapToGovernorateDto(Governorate governorate);
        CityDto MapToCityDto(City city);
        SpecializationDto MapToSpecializationDto(Specialization specialization);
        DoctorDto MapToDoctorDto(Doctor doctor);
        DoctorProfileDto MapToDoctorProfileDto(Doctor doctor);
        ReviewDto MapToReviewDto(Review review);
        Review MapToReview(AddReviewDto addReviewDto);
        PaymentInfoDto MapToPaymentInfoDto(Payment payment);
        Payment MapToPayment(AddPaymentDto addPaymentDto);
        AppointmentDetailsDto MapToAppointmentDetailsDto(Appointment appointment);
        Appointment MapToAppointment(BookAppointmentDto bookAppointmentDto , Guid userId);
        Appointment MapUpadateToAppointment(UpdateAppointmentDto updateAppointmentDto);
        UserProfileDto MapToUserProfileDto(ApplicationUser user);
        NotificationDto MapToNotificationDto(Notification notification);
        MedicalRecordDto MapToMedicalRecordDto(MedicalRecord medicalRecord);
        HomePageDto MapToHomePageDto(string userFullName, IEnumerable<Doctor> doctors, IEnumerable<Specialization> specializations);
    }
}
