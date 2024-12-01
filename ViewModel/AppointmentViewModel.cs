using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.ViewModel
{
    public class AppointmentViewModel
    {
        public NewAppointment AppointmentObj {get; set;}

        public List<NewAppointment> NewAppointments {get; set;}
        public List<string> AvailableServices {get; set;}
    }
}
