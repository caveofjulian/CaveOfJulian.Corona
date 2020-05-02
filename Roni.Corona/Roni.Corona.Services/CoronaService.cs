using Roni.Corona.Persistence;
using Roni.Corona.Shared;
using Roni.Corona.Services.Extensions;
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

        public IEnumerable<CaseReport> GetCases(ReportType reportType)
        {
            var cases = _coronaRepository.Get();
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType);
        }

        public IEnumerable<CaseReport> GetCases(ReportType reportType, DateTime date)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType);
        }

        public IEnumerable<CaseReport> GetCases(ReportType reportType, DateTime beginDate, DateTime endDate)
        { 
            var cases = _coronaRepository.Get()
                .Where(x => x.Date.Date >= beginDate.Date && x.Date.Date <= endDate.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType); ;
        }

        public IEnumerable<CaseReport> GetCases(ReportType reportType, string country)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Country == country);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType); ;
        }

        public IEnumerable<CaseReport> GetCases(ReportType reportType, string country, DateTime date)
        {
            var cases= _coronaRepository.Get()
                .Where(x => x.Country == country && x.Date.Date == date.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType); ;
        }

        public IEnumerable<CaseReport> GetCases(ReportType reportType, string country, DateTime beginDate, DateTime endDate)
        {
            var cases = _coronaRepository.Get()
                .Where(x => x.Country == country && x.Date.Date >= beginDate.Date && x.Date.Date <= endDate.Date);
            
            return _mapper.Map<IEnumerable<Cases>, IEnumerable<CaseReport>>(cases).ToReportType(reportType); ;
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
            await _coronaRepository.SaveAsync();
        }
    }
}
