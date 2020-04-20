using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Roni.Corona.Persistence.Entities;

namespace Roni.Corona.Services
{
    public interface ICoronaService
    {
        DateTime GetLastUpdated();
        IEnumerable<Cases> GetCases();
        IEnumerable<Cases> GetCases(DateTime date);
        IEnumerable<Cases> GetCases(string country);
        IEnumerable<Cases> GetCases(string country, DateTime date);
        int GetTotalConfirmedCases();
        int GetTotalRecoveredCases();
        int GetTotalDeathCases();
        Task InsertAsync(IEnumerable<Cases> cases);

    }
}