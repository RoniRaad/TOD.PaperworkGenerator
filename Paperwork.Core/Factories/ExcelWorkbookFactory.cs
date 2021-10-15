using ClosedXML.Excel;
using Paperwork.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Paperwork.Core.Factories
{
    public class ExcelWorkbookFactory : IExcelWorkbookFactory
    {
        public IXLWorkbook GetPaperworkTemplateWorkbook()
        {
            string fileName = "Equipment Form Template.xlsx";
            string path = Path.Combine(Environment.CurrentDirectory, @"Excel Sheets\", fileName);

            IXLWorkbook workbook = new XLWorkbook(path);

            return workbook;
        }
    }
}
