using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roni.Corona.DataIngestion.Integrations
{
    public interface ICoronaIntegration
    {
        Task<Dictionary<DateTime, string>> GetNewContent(DateTime lastUpdated);
    }
}