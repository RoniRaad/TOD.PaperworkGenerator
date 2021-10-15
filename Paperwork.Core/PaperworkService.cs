using System;
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
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;

namespace Paperwork.Core
{
    public class PaperworkService : IPaperworkService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IIOService _iOService;
        private readonly IEmptyExcelService _excelService;

        public PaperworkService(IDatabaseService databaseService, IIOService iOService, IEmptyExcelService excelService)
        {
            _databaseService = databaseService;
            _iOService = iOService;
            _excelService = excelService;
        }

        public PaperworkResponse GeneratePaperwork(List<PaperworkRequest> paperworkRequests)
        {
            List<string> infoMessages = new List<string>();
            List<string> errorMessages = new List<string>();

            var tempDirName = _iOService.GetTemporaryDirectory();

            foreach (var request in paperworkRequests)
            {
                try
                {
                    var trackitInfo = _databaseService.GetTrackitItemInfo(request.TOD);

                    if (trackitInfo is null)
                    {
                        errorMessages.Add($"Error: {request.TOD} not found in TrackIt database! Skipping.");
                        continue;
                    }


                    _excelService.OpenNewWorkbook(infoMessages, errorMessages)
                        .SetFields(trackitInfo, request)
                        .Save(tempDirName);
                }
                catch (Exception e)
                {
                    errorMessages.Add($"Error: An error occured while creating paperwork for {request.TOD}. Skipping.");
                }
            }

            _iOService.ZipFolderIntoFile(tempDirName, tempDirName + ".zip");

            return new PaperworkResponse
            {
                Info = infoMessages,
                Errors = errorMessages,
                FileName = Path.GetFileName(tempDirName + ".zip")
            }; ;
        }
    }
}
