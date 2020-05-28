using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Roni.Corona.DataIngestion.Integrations;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Services;

namespace Roni.Corona.DataIngestion
{
    internal class Ingester
    {
        private readonly ICoronaIntegration _integration;
        private readonly ICoronaService _service;
        private readonly ILogger<Ingester> _logger;
        
        // From 22nd of march, the indexes have changed..
        private readonly DateTime _newDate = new DateTime(2020, 3, 22);
        private readonly int[] _oldIndices = {0, 1, 2, 3, 4, 5};
        private readonly int[] _newIndices = {2, 3, 4, 7, 8, 9};

        internal Ingester(ICoronaIntegration integration, ICoronaService service, ILogger<Ingester> logger)
        {
            _integration = integration;
            _service = service;
            _logger = logger;
        }

        public async Task CheckForUpdates()
        {
            var lastUpdated = _service.GetLastUpdated();

            if (lastUpdated.Date <= DateTime.Now.Date)
            {
                var newContent = await _integration.GetNewContent(lastUpdated);

                foreach (var keyValuePair in newContent)
                {
                    var cases = MapStringToMultipleCases(keyValuePair.Value, keyValuePair.Key);
                    await InsertAsync(cases);
                     _logger.Log(LogLevel.Information, $"Successfully ingested {keyValuePair.Key}");
                }
            }

            _logger.Log(LogLevel.Information, "Finished ingestion successfully.");
        }

        private IEnumerable<Cases> MapStringToMultipleCases(string content, DateTime date)
        {
            var lines = content.Split("\n").Skip(1).ToArray();

            IList<Cases> cases = lines.Select(line => date.Date >= _newDate.Date
                                                  ? MapStringToCases(_newIndices, line, date)
                                                  : MapStringToCases(_oldIndices, line, date))
                                      .ToList();

            return cases.Where(x => x != null);
        }

        private Cases MapStringToCases(IReadOnlyList<int> indices, string content, DateTime date)
        {
            if (string.IsNullOrEmpty(content)) return null;

            var cells = SplitWithDoubleQuotes(',',content);

            var isDateParsed = DateTime.TryParseExact(cells[indices[2]], "d/MM/yyyy HH:mm", null,
                                                       DateTimeStyles.AllowWhiteSpaces, out var lastDate);
            if (!isDateParsed) lastDate = date;

            if (int.TryParse(cells[indices[0]], out var fips))
            {
                return new Cases
                {
                    State = cells[indices[2]],
                    Country = cells[indices[3]],
                    LastUpdated = DateTime.Parse(cells[indices[4]]),
                    Confirmed = cells[indices[7]].ParseToInt(),
                    Death = cells[indices[8]].ParseToInt(),
                    Recovered = cells[indices[9]].ParseToInt(),
                    Date = date
                };
            }

            return new Cases
            {
                State = cells[indices[0]],
                Country = cells[indices[1]],
                LastUpdated = lastDate,
                Confirmed = cells[indices[3]].ParseToInt(),
                Death = cells[indices[4]].ParseToInt(),
                Recovered = cells[indices[5]].ParseToInt(),
                Date = date
            };
        }

        private static string[] SplitWithDoubleQuotes(char delimiter, string line)
        {
            var result = new List<string>();
            var builder = new StringBuilder("");

            var inQuotes = false;

            foreach (var part in line)
            {
                if (part == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (part == delimiter) 
                {
                    if (!inQuotes) 
                    {
                        result.Add(builder.ToString());
                        builder.Clear();
                    }
                    else
                        builder.Append(part); 
                }
                else
                {
                    builder.Append(part);
                }
            }
            result.Add(builder.ToString());
            return result.ToArray(); 
        }

        private async Task InsertAsync(IEnumerable<Cases> data)
        {
            await _service.InsertAsync(data);
        }
    }
}