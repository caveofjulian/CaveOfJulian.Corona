using Roni.Corona.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roni.Corona.Persistence.Entities;

namespace Roni.Corona.Services
{

    public class CoronaService : ICoronaService
    {

        private readonly ICoronaCasesRepository<Cases> _coronaRepository;

        public CoronaService(ICoronaCasesRepository<Cases> coronaRepository)
        {
            _coronaRepository = coronaRepository;
        }

        public DateTime GetLastUpdated()
        {
            var confirmedCases = _coronaRepository.Get()?.OrderBy(x => x.Date)?.Take(1);
            
            return confirmedCases.Any() ? confirmedCases.First().Date : DateTime.MinValue;
        }

        public IEnumerable<Cases> GetCases()
        {
            return _coronaRepository.Get();
        }

        public IEnumerable<Cases> GetCases(DateTime date)
        {
            return _coronaRepository.Get().Where(x => x.Date.Date == date.Date);
        }

        public IEnumerable<Cases> GetCases(string country)
        {
            return _coronaRepository.Get().Where(x => x.Country == country);
        }

        public IEnumerable<Cases> GetCases(string country, DateTime date)
        {
            return _coronaRepository.Get().Where(x => x.Country == country && x.Date.Date == date.Date);
        }

        public int GetTotalConfirmedCases()
        {
            return _coronaRepository.Get().Sum(x => x.Confirmed);
        }

        public int GetTotalRecoveredCases()
        {
            return _coronaRepository.Get().Sum(x => x.Recovered);
        }

        public int GetTotalDeathCases()
        {
            return _coronaRepository.Get().Sum(x => x.Death);
        }

        public async Task InsertAsync(IEnumerable<Cases> cases)
        {
            await _coronaRepository.InsertAsync(cases);
        }

    }
}
