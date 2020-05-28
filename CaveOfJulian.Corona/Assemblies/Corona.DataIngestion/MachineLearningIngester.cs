using System;
using System.Collections.Generic;
using System.Linq;
using Corona.Services;
using Corona.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Corona.DataIngestion
{
    public class MachineLearningIngester : IDisposable
    {
        private readonly ILogger _logger;
        private readonly ICoronaService _service;
        private readonly SqlConnection _connection;

        private const string _insertQuery = @"
INSERT INTO [dbo].[MLCases]
           ([Country]
           ,[Date]
           ,[Confirmed]
           ,[Death]
           ,[Recovered])
     VALUES
           (@Country
           ,@Date
           ,@Confirmed
           ,@Death
           ,@Recovered)

        ";

        public MachineLearningIngester(ILogger logger, SqlConnection connection, ICoronaService service)
        {
            _logger = logger;
            _service = service;
            _connection = connection;
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        /// <summary>
        /// Ingests all data from corona database into machine learning database from starting date to current date.
        /// </summary>
        /// <param name="startDate">Start date to ingest from.</param>
        /// <returns></returns>
        public void Ingest(DateTime startDate)
        {
            var currentDate = startDate;

            _logger.LogInformation(currentDate >= DateTime.Now.Date
                ? "Ingestion could not be started as given startdate is in the future."
                : "Ingestion has started.");

            while (currentDate.Date < DateTime.Now.Date)
            {
                try
                {
                    var cases = _service.GetCases(new CoronaParameters() {Date = currentDate}, ReportType.All);
                    Insert(ToMlCases(cases, currentDate));
                    currentDate = currentDate.AddDays(1);
                    _logger.LogInformation($"Ingestion for date {currentDate.Date} has been successfully excuted.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e,$"Ingestion failed for date {currentDate.Date}");
                }
            }
        }

        private static IEnumerable<MLCase> ToMlCases(IEnumerable<CaseReport> reports, DateTime date)
        {
            foreach (var country in reports.Select(x => x.Country).Distinct())
            {
                var cases = reports.Where(x => x.Country == country);

                yield return new MLCase()
                {
                    Country = country,
                    Date = date,
                    Death = cases.Sum(x => x.Death),
                    Confirmed = cases.Sum(x => x.Confirmed),
                    Recovered = cases.Sum(x => x.Recovered)
                };   
            }
        }

        private void Insert(IEnumerable<MLCase> cases)
        {
            foreach (var mlCase in cases)
            {
                using var cmd = _connection.CreateCommand();

                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("country", mlCase.Country);
                cmd.Parameters.AddWithValue("date", mlCase.Date);
                cmd.Parameters.AddWithValue("confirmed", mlCase.Confirmed);
                cmd.Parameters.AddWithValue("death", mlCase.Death);
                cmd.Parameters.AddWithValue("recovered", mlCase.Recovered);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
