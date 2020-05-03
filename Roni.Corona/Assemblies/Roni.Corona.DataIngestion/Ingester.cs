using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Roni.Corona.DataIngestion.Integrations;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Services;
using Roni.Corona.Shared;

namespace Roni.Corona.DataIngestion
{
    internal class Ingester
    {
        private readonly ICoronaIntegration _integration;
        private readonly ICoronaService _service;
        private readonly ILogger<Ingester> _logger;
        private readonly IMapper _mapper;

        private readonly int _intervalInMinutes = 60;


        // From 22nd of march, the indexes have changed..
        private readonly DateTime _newDate = new DateTime(2020,3,22);
        private readonly int[] _oldIndices = { 0,1,2,3,4,5};
        private readonly int[] _newIndices = { 2,3,4,7,8,9};
        
        internal Ingester(ICoronaIntegration integration, ICoronaService service, ILogger<Ingester> logger, IMapper mapper)
        {
            _integration = integration;
            _service = service;
            _logger = logger;
        }

        public async Task CheckForUpdates()
        {
            var lastUpdated = _service.GetLastUpdated();

            if (lastUpdated.Date < DateTime.Now.Date)
            {
                var newContent = await _integration.GetNewContent(lastUpdated);
                
                foreach (var keyValuePair in newContent)
                {
                    var cases = MapStringToMultipleCases(keyValuePair.Value, keyValuePair.Key);
                    await UpdateAsync(cases);
                    _logger.Log(LogLevel.Information, $"Successfully ingested {keyValuePair.Key}");
                }
            }
            _logger.Log(LogLevel.Information, "Finished ingestion successfully.");
        }

        private IEnumerable<Cases> MapStringToMultipleCases(string content, DateTime date)
        {
            IList<Cases> cases = new List<Cases>();

            var lines = content.Split(Environment.NewLine).Skip(1).ToArray();

            foreach (var line in lines)
            {
                var mappedCases = date.Date >= _newDate.Date
                    ? MapStringToCases(_newIndices, line, date)
                    : MapStringToCases(_oldIndices, line, date);

                 cases.Add(mappedCases);    
            }

            return cases.Where(x => x!=null);
        }

        private Cases MapStringToCases(int[] indices, string content, DateTime date)
        {
            if (string.IsNullOrEmpty(content)) return null;

            var cells = content.Split(",");

            var confirmed = cells[indices[3]].ParseToInt();
            var deaths = cells[indices[4]].ParseToInt();
            var recovered = cells[indices[5]].ParseToInt();

            var isDateParsed = DateTime.TryParseExact(cells[indices[2]], "d/MM/yyyy HH:mm", null,DateTimeStyles.AllowWhiteSpaces, out var lastDate);
            if (!isDateParsed) lastDate = date;
            
            return new Cases()
            {
                State = cells[indices[0]],
                Country = cells[indices[1]],
                LastUpdated = lastDate,
                Confirmed = confirmed,
                Death = deaths,
                Recovered = recovered,
                Date = date
            };
        }

        private async Task UpdateAsync(IEnumerable<Cases> data)
        {
            await _service.InsertAsync(_mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(data));
        }
    }
}
