using Roni.Corona.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roni.Corona.Services.Extensions
{
    public static class ReportExtensions
    {
        /// <summary>
        /// Changes the report to the desired format. Might have some overhead wrt RAM, but currently a non-issue.
        /// </summary>
        /// <param name="reportType">Type of the report</param>
        /// <param name="report">Report for corona cases</param>
        /// <returns></returns>
        public static CaseReport ToReportType(this CaseReport report, ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Confirmed:
                    report.Death = null;
                    report.Recovered = null;
                    break;
                case ReportType.Death:
                    report.Confirmed = null;
                    report.Recovered = null;
                    break;
                case ReportType.Recovered:
                    report.Confirmed = null;
                    report.Death = null;
                    break;
            }

            // If type is all it will return the whole report immediately
            return report;
        }

        /// <summary>
        /// Changes the reports to the desired formats. Might have some overhead wrt RAM, but currently a non-issue.
        /// </summary>
        /// <param name="reportType">Type of the report</param>
        /// <param name="reports">Reports for corona cases</param>
        /// <returns></returns>
        public static IEnumerable<CaseReport> ToReportType(this IEnumerable<CaseReport> reports, ReportType reportType)
        {
            return reports.Select(report => report.ToReportType(reportType));
        }
    }
}
