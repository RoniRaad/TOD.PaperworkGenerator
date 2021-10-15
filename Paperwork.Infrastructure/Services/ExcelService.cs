using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Paperwork.Core.Enums;
using Paperwork.Core.Extensions;
using Paperwork.Core.Interfaces;
using Paperwork.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperwork.Infrastructure.Services
{
    public class ExcelService : IEmptyExcelService, IExcelService
    {
        private readonly IExcelWorkbookFactory _workbookFactory;
        private readonly IIOService _iOService;
        private IXLWorkbook workbook;
        private IXLWorksheet worksheet;
        private IList<string> info;
        private IList<string> errors;
        private TrackitEquipment trackitEquipment;
        private PaperworkRequest paperworkRequest;

        public ExcelService(IExcelWorkbookFactory workbookFactory, IIOService iOService)
        {
            _workbookFactory = workbookFactory;
            _iOService = iOService;
        }

        public IExcelService OpenNewWorkbook(List<string> infoList, List<string> errorList)
        {
            workbook = _workbookFactory.GetPaperworkTemplateWorkbook();
            worksheet = workbook.Worksheets.First();
            info = infoList;
            errors = errorList;

            return this;
        }

        public IExcelService SetFields(TrackitEquipment trackit, PaperworkRequest paperwork)
        {
            trackitEquipment = trackit;
            paperworkRequest = paperwork;

            worksheet.ReplaceValueInSheet("{TrackitNum}", trackitEquipment.WorkOrderNum);
            worksheet.ReplaceValueInSheet("{TODNum}", paperworkRequest.TOD);
            worksheet.ReplaceValueInSheet("{SerialNum}", trackitEquipment.ServiceTag);
            worksheet.SetToAndFromField("User", trackitEquipment.CurrentUser, paperworkRequest.User);
            worksheet.SetToAndFromField("Dept", trackitEquipment.Department, paperworkRequest.Department);
            worksheet.SetToAndFromField("Location", trackitEquipment.Location, paperworkRequest.Location);
            worksheet.ReplaceValueInSheet("{Notes}", paperworkRequest.Notes is null ? "" : paperworkRequest.Notes);
            worksheet.ReplaceValueInSheet("{TechName}", paperworkRequest.Requestor is null ? "" : paperworkRequest.Requestor);
            worksheet.ReplaceValueInSheet("{CurrentDate}", DateTime.Now.ToString("d"));
            worksheet.ReplaceValueInSheet("{AuctionNum}", "");

            if (paperworkRequest?.Location?.ToLower()?.Contains("eol") == true || paperworkRequest?.Location?.ToLower()?.Contains("end of life") == true)
            {
                worksheet.Cell($"Q{(int)Checkboxes.EOL_Auction + 1}").Value = true;
            }

            SetCheckboxes();

            if (trackitEquipment.WorkOrderNum != "N/A")
                info.Add($"Info: Work order found for {paperworkRequest.TOD}, Ensure you add the paperwork to the TrackIt ticket!");

            return this;
        }

        public IExcelService SetCheckboxes()
        {
            Checkboxes checkbox;

            if (Enum.TryParse<Checkboxes>(trackitEquipment.Type, out checkbox))
            {
                worksheet.Cell($"Q{(int)checkbox + 1}").Value = true;
                worksheet.ReplaceValueInSheet("{OtherDesc}", "");
            }
            else
            {
                worksheet.Cell($"Q{(int)Checkboxes.Other + 1}").Value = true;
                worksheet.ReplaceValueInSheet("{OtherDesc}", trackitEquipment.Type);
            }

            return this;
        }

        public IEmptyExcelService Save(string dir)
        {
            
            workbook.SaveAs($"{dir}\\{paperworkRequest.Requestor}\\{((paperworkRequest.User == null) ? trackitEquipment.CurrentUser : paperworkRequest.User)}_{paperworkRequest.TOD}_{trackitEquipment.Type}.xlsx");

            workbook.Dispose();

            return this;
        }
    }
}
