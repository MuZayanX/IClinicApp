using IClinicApp.API.Dtos.Notifications;

namespace IClinicApp.API.Repos.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId);
        Task<bool> MarkNotificationAsReadAsync(Guid notificationId);
    }
}
