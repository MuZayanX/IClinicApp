using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
