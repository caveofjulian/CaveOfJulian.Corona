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
        IEnumerable<CaseReport> GetCases();
        IEnumerable<CaseReport> GetCases(DateTime date);
        IEnumerable<CaseReport> GetCases(DateTime beginDate, DateTime endDate);
        IEnumerable<CaseReport> GetCases(string country);
        IEnumerable<CaseReport> GetCases(string country, DateTime date);
        IEnumerable<CaseReport> GetCases(string country, DateTime beginDate, DateTime endDate);

        int GetTotalConfirmedCases();
        int GetTotalRecoveredCases();
        int GetTotalDeathCases();
        Task InsertAsync(IEnumerable<CaseReport> cases);
    }
}