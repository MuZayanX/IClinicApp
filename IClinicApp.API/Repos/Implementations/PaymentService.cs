using System.Security.Claims;
using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Payments;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Enums;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class PaymentService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDtoMapper dtoMapper) : IPaymentService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IDtoMapper _dtoMapper = dtoMapper;
        public async Task<PaymentInfoDto> CreatePaymentForAppointmentAsync(AddPaymentDto paymentDto, ClaimsPrincipal user)
        {
            var userIdString = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var userId = Guid.Parse(userIdString);
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == paymentDto.AppointmentId && a.UserId == userId);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found or you don't have permission to access it.");
            }
            // Check if the appointment is already paid
            var isPaid = await _context.Payments
                .AnyAsync(p => p.AppointmentId == paymentDto.AppointmentId);
            if (isPaid)
            {
                throw new InvalidOperationException("This appointment has already been paid for.");
            }
            if (appointment.Status != AppointmentStatus.Pending)
            {
                throw new InvalidOperationException("You can only pay for pending appointments.");
            }

            // Create the payment entity
            var payment = _dtoMapper.MapToPayment(paymentDto);
            // Add the payment to the context and save changes
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            // Update the appointment status to Paid
            
            return _dtoMapper.MapToPaymentInfoDto(payment);

        }
    }
}
