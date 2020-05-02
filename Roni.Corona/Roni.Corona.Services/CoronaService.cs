using Roni.Corona.Persistence;
using Roni.Corona.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roni.Corona.Persistence.Entities;
using AutoMapper;

namespace Roni.Corona.Services
{

    public class CoronaService : ICoronaService
    {

        private readonly ICoronaCasesRepository<Cases> _coronaRepository;
        private readonly Mapper _mapper;

        public CoronaService(ICoronaCasesRepository<Cases> coronaRepository, Mapper mapper)
        {
            _coronaRepository = coronaRepository;
            _mapper = mapper;
        }

        public DateTime GetLastUpdated()
        {
            var confirmedCases = _coronaRepository.Get()?.OrderBy(x => x.Date)?.Take(1);
            
            return confirmedCases.Any() ? confirmedCases.First().Date : DateTime.MinValue;
        }

        public IEnumerable<CaseReport> GetCases()
        {
            var cases = _coronaRepository.Get();
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
        }

        public IEnumerable<CaseReport> GetCases(DateTime date)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
        }

        public IEnumerable<CaseReport> GetCases(DateTime beginDate, DateTime endDate)
        { 
            var cases = _coronaRepository.Get()
                .Where(x => x.Date.Date >= beginDate.Date && x.Date.Date <= endDate.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
        }

        public IEnumerable<CaseReport> GetCases(string country)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Country == country);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
        }

        public IEnumerable<CaseReport> GetCases(string country, DateTime date)
        {
            var cases= _coronaRepository.Get()
                .Where(x => x.Country == country && x.Date.Date == date.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
        }

        public IEnumerable<CaseReport> GetCases(string country, DateTime beginDate, DateTime endDate)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Country == country && x.Date.Date >= beginDate.Date && x.Date.Date <= endDate.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases);
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

        public async Task InsertAsync(IEnumerable<CaseReport> cases)
        {
            var entities =  _mapper.Map<IEnumerable<CaseReport>, IEnumerable<Cases>>(cases);
            await _coronaRepository.InsertAsync(entities);
        }

    }
}
