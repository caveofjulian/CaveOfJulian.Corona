using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Corona.Persistence;
using Corona.Persistence.Entities;
using Corona.Services.Extensions;
using Corona.Shared;

namespace Corona.Services
{

    public class CoronaService : ICoronaService
    {

        private readonly ICoronaCasesRepository<Cases> _coronaRepository;
        private readonly IMapper _mapper;

        public CoronaService(ICoronaCasesRepository<Cases> coronaRepository, IMapper mapper)
        {
            _coronaRepository = coronaRepository;
            _mapper = mapper;
        }

        public DateTime GetLastUpdated()
        {
            var confirmedCases = _coronaRepository.Get()?.OrderBy(x => x.Date)?.Take(1);
            
            return confirmedCases.Any() ? confirmedCases.First().Date : DateTime.MinValue;
        }

        public IEnumerable<CaseReport> GetCases(CoronaParameters parameters, ReportType reportType)
        {
            var cases = _coronaRepository.Get()
                    .Where(x => parameters.Country == default || x.Country == parameters.Country)
                    .Where(x => parameters.Date == default || x.Date == parameters.Date)
                    .Where(x => parameters.BeginDate == default && parameters.EndDate == default ||
                                (x.Date >= parameters.BeginDate && x.Date <= parameters.EndDate));

            return _mapper.Map<IEnumerable<CaseReport>>(cases).ToReportType(reportType);
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
            await _coronaRepository.SaveAsync();
        }
    }
}
