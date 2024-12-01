using HairSalon.Dtos;
using HairSalon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HairSalon.ViewModel
{
   
    public class ReportsViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}
