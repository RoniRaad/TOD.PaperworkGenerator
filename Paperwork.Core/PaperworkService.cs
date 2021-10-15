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
            var tempDirName = _iOService.GetTemporaryDirectory();
            PaperworkResponse response = new PaperworkResponse() { FileName = tempDirName + ".zip"};

            foreach (var request in paperworkRequests)
            {
                var trackitInfo = _databaseService.GetTrackitItemInfo(request.TOD);

                response = _excelService.OpenNewWorkbook()
                    .SetFields(trackitInfo, request)
                    .Save(tempDirName);

            }

            _iOService.ZipFolderIntoFile(tempDirName, tempDirName + ".zip");

            return response;
        }
    }
}
