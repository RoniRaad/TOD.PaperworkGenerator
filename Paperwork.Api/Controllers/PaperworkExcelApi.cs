using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Paperwork.Core;
using Paperwork.Core.Models;
using Paperwork.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Paperwork.Api.Controllers
{
    [Authorize(Roles = "davie-fl\\BomgarHelpDesk")]
    [ApiController]
    [Route("[controller]")]
    public class PaperworkExcelApi : ControllerBase
    {
        private readonly ILogger<PaperworkExcelApi> _logger;
        private readonly IExcelService _excelService;

        public PaperworkExcelApi(ILogger<PaperworkExcelApi> logger, IExcelService excelService)
        {
            _logger = logger;
            _excelService = excelService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<PaperworkRequest> paperworkRequests)
        {
            _logger.LogInformation("Paperwork requested! Attempting to generate paperwork.");
            try 
            { 
                PaperworkResponse tempFileName = _excelService.GeneratePaperwork(paperworkRequests);

                return new JsonResult(tempFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured generating paperwork!");
                _logger.LogError(ex?.Message);
                _logger.LogError(ex?.InnerException?.Message);
                _logger.LogError(ex?.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
