﻿    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace SamstedHotel.Model
    {
        public class Reservation
        {
            public int ReservationID { get; set; }
            public int CustomerID { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public string BookingType { get; set; } 
        }
    }
