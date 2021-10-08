using Paperwork.Core.Models;
using System.Collections.Generic;

namespace Paperwork.Core.Interfaces
{
    public interface IExcelService
    {
        PaperworkResponse GeneratePaperwork(List<PaperworkRequest> paperworkRequests);
        string GetTemporaryDirectory();
        TrackitEquiptment GetTrackitEquiptmentFromRequest(PaperworkRequest pwRequest);
    }
}