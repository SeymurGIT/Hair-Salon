namespace HairSalon.ViewModel
{
    public class TotalCountsViewModel
    {
        public int TotalAppointments { get; set; }
        public int TotalServices{ get; set; }
        public int TotalCustomers { get; set; }
        public int TotalAcceptedAppointments { get; set; }
        public int TotalSubscribers { get; set; }
        public int TotalRejectedAppointments { get; set; }
        public int TodayCustomers { get; set; }
        public int TodaysAppointments { get; set; }

        public int WeeksAppointments { get; set; }

        public int YesterdaysAppointments { get; set; }
    }
}
