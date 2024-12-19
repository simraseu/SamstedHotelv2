using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel.Model
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int RoomTypeID { get; set; }
        public string Status { get; set; } = "Available";

        // New property to hold the Room Type name
        public string RoomTypeName { get; set; } = string.Empty;
    }
}
