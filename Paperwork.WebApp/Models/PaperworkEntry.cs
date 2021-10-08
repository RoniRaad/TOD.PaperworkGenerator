using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paperwork.WebApp.Models
{
    public class PaperworkEntry
    {
        public string TOD { get; set; }
        public string Department { get; set; }
        public string User { get; set; }
        public string Location { get; set; }
        public string Requestor { get; set; }
        public string Notes { get; set; }
    }
}
