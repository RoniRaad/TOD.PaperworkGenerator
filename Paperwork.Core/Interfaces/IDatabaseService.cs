using Paperwork.Core.Models;
using System.Collections.Generic;

namespace Paperwork.Core.Interfaces
{
    public interface IDatabaseService
    {
        IList<string> GetDepartments();
        IList<string> GetLocations();
        TrackitEquipment GetTrackitItemInfo(string todNum);
        IList<string> GetUsers();
        string GetWorkOrderNumber(string todNum);
    }
}