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
        IEnumerable<CaseReport> GetCases(ReportType reportType);
        IEnumerable<CaseReport> GetCases(ReportType reportType, DateTime date);
        IEnumerable<CaseReport> GetCases(ReportType reportType, DateTime beginDate, DateTime endDate);
        IEnumerable<CaseReport> GetCases(ReportType reportType, string country);
        IEnumerable<CaseReport> GetCases(ReportType reportType, string country, DateTime date);
        IEnumerable<CaseReport> GetCases(ReportType reportType, string country, DateTime beginDate, DateTime endDate);

        int GetTotalConfirmedCases();
        int GetTotalRecoveredCases();
        int GetTotalDeathCases();
        Task InsertAsync(IEnumerable<CaseReport> cases);
    }
}