using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperwork.Core.Models
{
    public class PaperworkRequest
    {
        private string tod;
        public string TOD
        {
            get => tod;
            set => tod = (!(value.ToUpper()).StartsWith("TOD")) ? "TOD" + value : value.ToUpper();
        }
        public string Department { get; set; }
        public string User { get; set; }
        public string Location { get; set; }
        public string Requestor { get; set; }
        public string Notes { get; set; }
    }
}
