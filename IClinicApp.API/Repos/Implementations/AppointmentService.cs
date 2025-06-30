using System.Security.Claims;
using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Appointments;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Enums;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class AppointmentService(ApplicationDbContext context, IDtoMapper dtoMapper, UserManager<ApplicationUser> userManager) : IAppointmentService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDtoMapper _dtoMapper = dtoMapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<AppointmentDetailsDto> BookAppointmentAsync(BookAppointmentDto bookAppointment, ClaimsPrincipal user)
        {
            var userIdString = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var userId = Guid.Parse(userIdString);

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == bookAppointment.DoctorId);
            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }
            //check if the requested data is in the past
            if (bookAppointment.Date <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Appointment date must be in the future.");
            }

            // check for double-booking (the most important validation)
            var isSlotTaken = await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == bookAppointment.DoctorId &&
                    a.Date == bookAppointment.Date &&
                    (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Payed) // Only check for pending or confirmed appointments
                );
            if (isSlotTaken)
            {
                throw new InvalidOperationException("This appointment slot is already taken.");
            }

            // Create the appointment Entity
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                Date = bookAppointment.Date,
                Status = AppointmentStatus.Pending,
                UserId = userId,
                DoctorId = bookAppointment.DoctorId
            };


            // save to database
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            // fetch the created appointment with all related data
            var createdAppointment = await _context.Appointments
                .Include(a => a.Doctor)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            if (createdAppointment == null)
            {
                throw new Exception("Failed to retrieve the created appointment.");
            }

            // map to DTO
            var appointmentDetailsDto = _dtoMapper.MapToAppointmentDetailsDto(createdAppointment);
            return appointmentDetailsDto;
        }
 

    public async Task<AppointmentDetailsDto> ConfirmAppointmentPaymentAsync(Guid appointmentId, ClaimsPrincipal doctorUser)
        {
            var doctorAppUserId = _userManager.GetUserId(doctorUser);
            if (string.IsNullOrEmpty(doctorAppUserId))
            {
                throw new UnauthorizedAccessException("Doctor is not authenticated.");
            }

            var doctorId = Guid.Parse(doctorAppUserId);
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == doctorId);
            if (doctor == null)
            {
                throw new UnauthorizedAccessException("Doctor not found or the user does not have permission to confirm this appointment.");
            }
            var appointment = await _context.Appointments
                .Include(a => a.Payment)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }
            if (appointment.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to confirm this appointment.");
            }
            if (appointment.Status != AppointmentStatus.Pending)
            {
                throw new InvalidOperationException("You can only confirm pending appointments.");
            }
            // Check if the appointment has a payment
            if (appointment.Payment == null)
            {
                throw new InvalidOperationException("This appointment does not have a payment associated with it.");
            }

            // Update the appointment status to Payed
            appointment.Payment.IsConfirmed = true;
            appointment.Status = AppointmentStatus.Payed;

            await _context.SaveChangesAsync();

            // Map to DTO
            var confirmedAppointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Payment)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            return _dtoMapper.MapToAppointmentDetailsDto(confirmedAppointment!);
        }
        public async Task<AppointmentDetailsDto> CompleteAppointmentAsync(Guid appointmentId, ClaimsPrincipal doctorUser)
        {
            var doctorAppUserId = _userManager.GetUserId(doctorUser);
            if (string.IsNullOrEmpty(doctorAppUserId))
            {
                throw new UnauthorizedAccessException("Doctor is not authenticated.");
            }
            var doctorId = Guid.Parse(doctorAppUserId);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == doctorId);
            if (doctor == null)
            {
                throw new UnauthorizedAccessException("Doctor not found or the user does not have permission to complete this appointment.");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Payment)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You do not have permission to complete this appointment.");
            }

            if (appointment.Status != AppointmentStatus.Payed)
            {
                throw new InvalidOperationException("You can only complete paid appointments.");
            }

            if (appointment.Date > DateTime.UtcNow)
            {
                throw new InvalidOperationException("You cannot complete an appointment that is scheduled for the future.");
            }

            // Update the appointment status to Completed
            appointment.Status = AppointmentStatus.Completed;
            await _context.SaveChangesAsync();
            // Map to DTO

            return _dtoMapper.MapToAppointmentDetailsDto(appointment);
        }


        public async Task CancelAppointmentAsPatientAsync(Guid appointmentId, ClaimsPrincipal user)
        {
            var userIdString = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = Guid.Parse(userIdString);

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found or you don't have permission to cancel it.");
            }

            if (appointment.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to cancel this appointment.");
            }

            if (appointment.Status != AppointmentStatus.Pending && appointment.Status != AppointmentStatus.Payed)
            {
                throw new InvalidOperationException("You can only cancel pending and payed appointments.");
            }

            var hoursUntilAppointment = (appointment.Date - DateTime.UtcNow).TotalHours;

            if (hoursUntilAppointment > 24)
            {
                appointment.Status = AppointmentStatus.CanceledByUser;
            }
            else
            {
                appointment.Status = AppointmentStatus.CanceledByUser;

                // TODO: Implement your business logic for a last-minute cancellation penalty.
                // For example, you could log this event to a separate table for reporting,
                // or add a "strike" against the user's account.
                // For now, we just change the status, but the logic to apply a penalty would go here.
                // e.g., await _penaltyService.ApplyLateCancellationPenalty(userId);
            }
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsDoctorAsync(Guid appointmentId, ClaimsPrincipal doctorUser)
        {
            var doctorAppUserId = _userManager.GetUserId(doctorUser);
            if (string.IsNullOrEmpty(doctorAppUserId))
            {
                throw new UnauthorizedAccessException("Doctor is not authenticated.");
            }

            var doctor = await _context.Doctors
                .AsNoTracking() // No need to track the doctor entity for this operation
                .FirstOrDefaultAsync(d => d.UserId == Guid.Parse(doctorAppUserId));

            if (doctor == null)
            {
                throw new UnauthorizedAccessException("The logged-in user is not a valid doctor.");
            }

            // === 2. FIND THE APPOINTMENT AND VALIDATE ===
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            // AUTHORIZATION: Ensure the appointment belongs to the doctor trying to cancel it.
            if (appointment.DoctorId != doctor.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to cancel this appointment.");
            }

            // === 3. VALIDATE APPOINTMENT STATE ===
            // A doctor can only cancel appointments that are upcoming.
            if (appointment.Status != AppointmentStatus.Pending && appointment.Status != AppointmentStatus.Payed)
            {
                throw new InvalidOperationException("This appointment cannot be canceled as it is already completed or canceled.");
            }

            // === 4. PERFORM THE UPDATE ===
            // Change the status to the specific "CanceledByDoctor" state.
            appointment.Status = AppointmentStatus.CanceledByDoctor;

            // === 5. SAVE THE CHANGES ===
            await _context.SaveChangesAsync();

            // === 6. CRITICAL: TRIGGER PATIENT NOTIFICATION ===
            // This is the most important part of this workflow from a user experience perspective.
            // You would inject a notification service and call it here to immediately inform the patient.
            // e.g., await _notificationService.SendAppointmentCanceledByDoctorNotification(appointment.UserId, appointment.Id);
        }
    }
}