using System;
using Corona.Services;
using Corona.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Corona.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoronaController : ControllerBase
    {
        private readonly ICoronaService _service;
        private readonly ILogger _logger;

        public CoronaController(ICoronaService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] CoronaParameters parameters)
        {
            return GetCases(parameters, ReportType.All);
        }

        [HttpGet("death")]
        public IActionResult GetDeathCases([FromQuery] CoronaParameters parameters)
        {
            return GetCases(parameters, ReportType.Death);
        }

        [HttpGet("confirmed")]
        public IActionResult GetConfirmedCases([FromQuery] CoronaParameters parameters)
        {
            return GetCases(parameters, ReportType.Confirmed);
        }


        [HttpGet("recovered")]
        public IActionResult GetRecoveredCases([FromQuery] CoronaParameters parameters)
        {
            return GetCases(parameters, ReportType.Recovered);
        }

        private IActionResult GetCases(CoronaParameters parameters, ReportType reportType)
        {
            try
            {
                return Ok(_service.GetCases(parameters, reportType));
            }
            catch (SqlException e)
            {
                _logger.LogInformation("Sql exception occured.", e);
                return StatusCode(500, "Database returned an error.");
            }
            catch (Exception e)
            {
                _logger.LogInformation("An unknown exception occured.", e);
                return StatusCode(500, "An unknown error has occurred.");
            }
        }

    }
}