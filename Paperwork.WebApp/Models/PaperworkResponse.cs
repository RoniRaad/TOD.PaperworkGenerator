using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Paperwork.Core.Models
{
    public class PaperworkResponse
    {
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
        [JsonPropertyName("info")]
        public List<string> Info { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
    
}
