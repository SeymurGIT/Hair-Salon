using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.ViewModel
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }

        public List<Service> AvailableServices { get; set; }
    }
}
