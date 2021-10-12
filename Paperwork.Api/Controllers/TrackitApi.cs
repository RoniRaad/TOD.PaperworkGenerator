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
using Paperwork.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Paperwork.Api.Controllers
{
    [Authorize(Roles = "davie-fl\\BomgarHelpDesk")]
    [ApiController]
    [Route("[controller]")]
    public class TrackitApi : ControllerBase
    {
        private readonly ILogger<DownloadPaperwork> _logger;
        private readonly IDatabaseService _dbService;

        public TrackitApi(ILogger<DownloadPaperwork> logger, IDatabaseService databaseService)
        {
            _logger = logger;
            _dbService = databaseService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Attempting to get users from TrackIt DB!");

            try
            {
                return new JsonResult(_dbService.GetUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured when attempting to get users from TrackIt DB!");
                _logger.LogError(ex?.Message);
                _logger.LogError(ex?.InnerException?.Message);
                _logger.LogError(ex?.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            _logger.LogInformation("Attempting to get departments from TrackIt DB!");

            try
            {
                return new JsonResult(_dbService.GetDepartments());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured when attempting to get departments from TrackIt DB!");
                _logger.LogError(ex?.Message);
                _logger.LogError(ex?.InnerException?.Message);
                _logger.LogError(ex?.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            _logger.LogInformation("Attempting to get locations from TrackIt DB!");

            try
            {
                return new JsonResult(_dbService.GetLocations());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured when attempting to get locations from TrackIt DB!");
                _logger.LogError(ex?.Message);
                _logger.LogError(ex?.InnerException?.Message);
                _logger.LogError(ex?.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
