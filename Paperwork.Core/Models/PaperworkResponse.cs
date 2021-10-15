using System.Collections.Generic;

namespace Paperwork.Core.Models
{
    public class PaperworkResponse
    {
        public IList<string> Errors { get; set; }
        public IList<string> Info { get; set; }
        public string FileName { get; set; }
    }
    
}
