using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamstedHotel.Model
{
    public class CourseRoom
    {
        public int CourseRoomID { get; set; }
        public string CourseRoomName { get; set; } = string.Empty;
        public string EventPackage {  get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }
}
