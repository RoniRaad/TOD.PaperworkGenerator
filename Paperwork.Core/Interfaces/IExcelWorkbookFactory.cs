using ClosedXML.Excel;

namespace Paperwork.Core.Interfaces
{
    public interface IExcelWorkbookFactory
    {
        IXLWorkbook GetPaperworkTemplateWorkbook();
    }
}