using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Shared;

namespace Roni.Corona.Services
{
    public interface ICoronaService
    {
        DateTime GetLastUpdated();
        IEnumerable<CaseReport> GetCases(CoronaParameters parameters, ReportType reportType);
        int GetTotalConfirmedCases();
        int GetTotalRecoveredCases();
        int GetTotalDeathCases();
        Task InsertAsync(IEnumerable<Cases> cases);
    }
}