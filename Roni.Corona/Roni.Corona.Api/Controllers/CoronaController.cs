using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Services;
using Roni.Corona.Shared;

namespace Roni.Corona.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoronaController : ControllerBase
    {
        private readonly ICoronaService _service;
        private readonly ILogger _logger;

        public CoronaController(ICoronaService service, ILogger<CoronaController> logger)
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
                return Ok(GetCaseReport(parameters, reportType));
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

        private IEnumerable<CaseReport> GetCaseReport(CoronaParameters parameters, ReportType reportType)
        {
            // Please stick to a chain of conditions like this so its very explicit. 
            if (parameters.Date == null && parameters.BeginDate == null && parameters.EndDate == null)
            {
                return _service.GetCases(reportType, parameters.Country);
            }
            if (parameters.Date != null && parameters.Country != null)
            {
                return _service.GetCases(reportType, parameters.Country, parameters.Date.GetValueOrDefault());
            }
            if (parameters.BeginDate != null && parameters.EndDate != null && parameters.Country == null)
            {
                return _service.GetCases(reportType, parameters.BeginDate.GetValueOrDefault(), parameters.EndDate.GetValueOrDefault());
            }
            if (parameters.BeginDate != null && parameters.EndDate != null && parameters.Country != null)            
            {
                return _service.GetCases(reportType, parameters.Country, parameters.BeginDate.GetValueOrDefault(), parameters.EndDate.GetValueOrDefault());
            }

            return null;
        }
    }
}