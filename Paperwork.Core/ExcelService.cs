﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Runtime.Intrinsics.X86;
using ClosedXML.Excel;
using System.Runtime.InteropServices;
using System.IO.Compression;
using Paperwork.Core.Enums;
using Paperwork.Core.Extensions;
using Paperwork.Core.Interfaces;
using Paperwork.Core.Models;

namespace Paperwork.Core
{
    public partial class ExcelService : IExcelService
    {
        private readonly IDatabaseService _databaseService;

        public ExcelService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public TrackitEquiptment GetTrackitEquiptmentFromRequest(PaperworkRequest pwRequest)
        {
            return _databaseService.GetTrackitItemInfo(pwRequest.TOD);
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public PaperworkResponse GeneratePaperwork(List<PaperworkRequest> paperworkRequests)
        {
            string saveDir = GetTemporaryDirectory();
            string fileName = "Copy of Blank Equipment Form.xlsx";
            string path = Path.Combine(Environment.CurrentDirectory, @"Excel Sheets\", fileName);
            List<string> info = new List<string>();
            List<string> errors = new List<string>();

            foreach (var request in paperworkRequests)
            {
                using (var workbook = new XLWorkbook(path))
                {
                    var sheet = workbook.Worksheets.First();

                    var trackitInfo = GetTrackitEquiptmentFromRequest(request);
                    var searchForWorkOrder = _databaseService.GetWorkOrderNumber(request.TOD);

                    if (trackitInfo is null)
                    {
                        errors.Add($"Error: {request.TOD} not found in trackit! This request has not been proccessed!");
                        continue;
                    }
                    if (searchForWorkOrder != "N/A")
                        info.Add($"Info: Work order found for {request.TOD}, Ensure you add the paperwork to the TrackIt ticket!");

                    sheet.ReplaceValueInSheet("{TrackitNum}", searchForWorkOrder);
                    sheet.ReplaceValueInSheet("{TODNum}", request.TOD);
                    sheet.ReplaceValueInSheet("{SerialNum}", trackitInfo.ServiceTag);
                    sheet.ReplaceValueInSheet("{AuctionNum}", "");
                    sheet.SetToAndFromField("User", trackitInfo.CurrentUser, request.User);
                    sheet.SetToAndFromField("Dept", trackitInfo.Department, request.Department);
                    sheet.SetToAndFromField("Location", trackitInfo.Location, request.Location);
                    sheet.ReplaceValueInSheet("{Notes}", request.Notes is null ? "" : request.Notes);
                    sheet.ReplaceValueInSheet("{TechName}", request.Requestor is null ? "" : request.Requestor);
                    sheet.ReplaceValueInSheet("{CurrentDate}", DateTime.Now.ToString("d"));
                    sheet.ReplaceValueInSheet("{OtherDesc}", "");
                    int checkbox = (int)Enum.Parse(typeof(Checkboxes), trackitInfo.Type);
                    sheet.Cell($"Q{checkbox + 1}").Value = true;
                    workbook.SaveAs($"{saveDir}\\{request.Requestor}\\{((request.User == null) ? trackitInfo.CurrentUser : request.User)}_{request.TOD}_{trackitInfo.Type}.xlsx");
                }
            }

            ZipFile.CreateFromDirectory(saveDir, saveDir + ".zip");

            (new DirectoryInfo(saveDir)).Delete(true);

            return new PaperworkResponse {
                Info = info,
                Errors = errors,
                FileName = Path.GetFileName(saveDir + ".zip")
             };
        }
    }
}
