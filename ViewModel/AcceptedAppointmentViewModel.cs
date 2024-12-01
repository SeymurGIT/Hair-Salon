using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.ViewModel
{
    public class AcceptedAppointmentViewModel
    {
        public NewAppointment AcceptedAppointmentObj {get; set;}
        public Dictionary<string, byte> ServicePrices { get; set; }
    }
}
