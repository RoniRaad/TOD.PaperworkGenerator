using System.Collections.Generic;

namespace Paperwork.Core.Models
{
    public class PaperworkResponse
    {
        public List<string> Errors { get; set; }
        public List<string> Info { get; set; }
        public string FileName { get; set; }
    }
    
}
