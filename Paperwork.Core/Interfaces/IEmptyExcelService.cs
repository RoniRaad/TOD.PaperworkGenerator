using Paperwork.Core.Models;
using System.Collections.Generic;

namespace Paperwork.Core.Interfaces
{
    public interface IEmptyExcelService
    {
        public IExcelService OpenNewWorkbook(List<string> infoList, List<string> errorList);
    }
}
