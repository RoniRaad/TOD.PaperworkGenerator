using Paperwork.Core.Models;

namespace Paperwork.Core.Interfaces
{
    public interface IEmptyExcelService
    {
        public IExcelService OpenNewWorkbook();
    }
}
