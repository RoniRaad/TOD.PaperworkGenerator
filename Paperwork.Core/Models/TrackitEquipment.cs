using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperwork.Core.Models
{
    public class TrackitEquipment
    {
        public string CurrentUser { get; set; }
        public string ServiceTag { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string TodNum { get; set; }
        public string Price { get; set; }
        public string WorkOrderNum { get; set; }
    }
}
