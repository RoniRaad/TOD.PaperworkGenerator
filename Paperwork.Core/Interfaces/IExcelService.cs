using Paperwork.Core.Models;

namespace Paperwork.Core.Interfaces
{
    public interface IExcelService
    {
        IExcelService SetFields(TrackitEquipment trackitEquipment, PaperworkRequest paperworkRequest);
        IExcelService SetCheckboxes();
        PaperworkResponse Save(string dir);
    }
}
