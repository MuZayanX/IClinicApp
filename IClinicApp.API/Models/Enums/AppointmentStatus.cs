using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Enums
{
    public enum AppointmentStatus
    {
        Pending = 0,
        Completed = 1,
        Payed = 2,
        CanceledByUser = 3,
        CanceledByDoctor = 4,
        Expired = 5
    }
}
