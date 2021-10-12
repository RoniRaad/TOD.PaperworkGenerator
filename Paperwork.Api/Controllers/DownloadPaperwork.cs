using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Paperwork.Api.Controllers
{
    [Authorize(Roles = "davie-fl\\BomgarHelpDesk")]
    [ApiController]
    [Route("[controller]")]
    public class DownloadPaperwork : ControllerBase
    {
        private readonly ILogger<DownloadPaperwork> _logger;

        public DownloadPaperwork(ILogger<DownloadPaperwork> logger)
        {
            _logger = logger;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            _logger.LogInformation($"Attempting to serve file {fileName} from the temp path.");

            try 
            {
                if (System.IO.Path.GetExtension(fileName) == ".zip")
                {
                    var filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);
                    if (!System.IO.File.Exists(filePath))
                        return BadRequest();
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);

                    System.IO.File.Delete(filePath);

                    return File(fileBytes,
                        "application/force-download",
                        $"{DateTime.Now.ToString("d-M-yy")}_Paperwork.zip");
                }

                _logger.LogError($"Failed to serve file, requested file was not a zip file!");

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured when attempting to serve a file!");
                _logger.LogError(ex?.Message);
                _logger.LogError(ex?.InnerException?.Message);
                _logger.LogError(ex?.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
