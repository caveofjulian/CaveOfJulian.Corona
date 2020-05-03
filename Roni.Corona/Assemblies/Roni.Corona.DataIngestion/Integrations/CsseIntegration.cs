using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Roni.Corona.DataIngestion.Integrations
{
    public class CsseIntegration : ICoronaIntegration
    {
        private readonly HttpClient _client;

        private const string _baseUrl =
            "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/";

        private DateTime LastUpdated { get; set; } = DateTime.Now.Subtract(TimeSpan.FromDays(1));

        public CsseIntegration(HttpClient client)
        {
            _client = client;
        }

        public async Task<Dictionary<DateTime, string>> GetNewContent(DateTime lastUpdated)
        {
            Dictionary<DateTime, string> csvData = new Dictionary<DateTime, string>();

            DateTime beginDate = new DateTime(2020,1,22);

            if (lastUpdated.Date < beginDate.Date)
            {
                lastUpdated = beginDate;
            }
                
            while (lastUpdated.Date < DateTime.Now.Date)
            {
                var downloadUrl = _baseUrl + lastUpdated.ToString("MM-dd-yyyy") + ".csv";
                var response = await _client.GetAsync(downloadUrl);
                var content = await response.Content.ReadAsStringAsync();
                
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
