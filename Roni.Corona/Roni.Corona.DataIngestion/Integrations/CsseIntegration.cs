using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Roni.Corona.DataIngestion.Integrations
{
    public class CsseIntegration : ICoronaIntegration
    {
        private readonly HttpClient _client;

        private const string BaseUrl =
            "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports";

        private DateTime LastUpdated { get; set; } = DateTime.Now.Subtract(TimeSpan.FromDays(1));

        public CsseIntegration(HttpClient client)
        {
            _client = client;
        }

        public async Task<Dictionary<DateTime, string>> GetNewContent(DateTime lastUpdated)
        {
            Dictionary<DateTime, string> csvData = new Dictionary<DateTime, string>();

            var beginDate = new DateTime(2020, 1, 22);

            if (lastUpdated.Date < beginDate.Date)
            {
                lastUpdated = beginDate;
            }

            while (lastUpdated.Date < DateTime.Now.Date)
            {
                string url = $"{BaseUrl}/{lastUpdated:MM-dd-yyyy}.csv";
                HttpResponseMessage res = await _client.GetAsync(url);
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    continue;
                }

                string content = await res.Content.ReadAsStringAsync();

                if (content != null)
                {
                    csvData.Add(lastUpdated, content);
                }

                lastUpdated = lastUpdated.AddDays(1);
            }

            return csvData;
        }
    }
}