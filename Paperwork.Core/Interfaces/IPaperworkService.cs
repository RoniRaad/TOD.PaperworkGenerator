using Paperwork.Core.Models;
using System.Collections.Generic;

namespace Paperwork.Core.Interfaces
{
    public interface IPaperworkService
    {
        PaperworkResponse GeneratePaperwork(List<PaperworkRequest> paperworkRequests);
    }
}