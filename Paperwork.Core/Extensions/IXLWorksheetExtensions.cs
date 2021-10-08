using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperwork.Core.Extensions
{

    public static class IXLWorksheetExtensions
    {
        public static bool ReplaceValueInSheet(this IXLWorksheet worksheet, string fromValue, string toValue)
{
            foreach (var cell in worksheet.Cells())
            {
                if (!(cell?.GetValue<string>() is null) && cell.GetValue<string>().Contains(fromValue))
                {
                    Console.WriteLine("Contains");
                    cell.Value = cell.GetValue<string>().Replace(fromValue, toValue);
                    return true;
                }
            }

            return false;
        }

        public static void SetToAndFromField(this IXLWorksheet worksheet, string fieldName, string fromValue, string toValue)
        {
            string fromField = "{From" + fieldName + "}";
            string toField = "{To" + fieldName + "}";

            worksheet.ReplaceValueInSheet(fromField, fromValue);

            if (toValue == fromValue)
                worksheet.ReplaceValueInSheet(toField, "");
            else
                worksheet.ReplaceValueInSheet(toField, toValue);

        }
    }
    
}
