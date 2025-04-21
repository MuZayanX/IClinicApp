using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Enums
{
    public enum NotificationType
    {
        AppointmentBooked,
        AppointmentCancelled,
        AppointmentCompleted,
        MedicalRecordAdded,
        PaymentConfirmed,
        Reminder,
        General
    }
}
