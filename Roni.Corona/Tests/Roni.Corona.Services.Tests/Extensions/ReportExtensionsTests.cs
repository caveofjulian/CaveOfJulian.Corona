using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using Roni.Corona.Services.Extensions;
using Roni.Corona.Shared;

namespace Roni.Corona.Services.Tests.Extensions
{
    public class ReportExtensionsTests
    {
        [Test]
        public void ToReportType_OneReport_ReturnsAllCaseReport()
        {
            // Arrange
            var fixture = new Fixture();
            var report = fixture.Create<CaseReport>();

            // Act
            var actualReport = report.ToReportType(ReportType.All);

            // Assert
            Assert.That(actualReport.Confirmed, Is.Not.Null);
            Assert.That(actualReport.Recovered, Is.Not.Null);
            Assert.That(actualReport.Recovered, Is.Not.Null);
        }

        [Test]
        public void ToReportType_OneReport_ReturnsDeathCaseReport()
        {
            // Arrange
            var fixture = new Fixture();
            var report = fixture.Create<CaseReport>();
            
            // Act
            var actualReport = report.ToReportType(ReportType.Death);

            // Assert
            Assert.That(actualReport.Confirmed, Is.Null);
            Assert.That(actualReport.Recovered, Is.Null);
        }

        [Test]
        public void ToReportType_OneReport_ReturnsConfirmedCaseReport()
        {
            // Arrange
            var fixture = new Fixture();
            var report = fixture.Create<CaseReport>();

            // Act
            var actualReport = report.ToReportType(ReportType.Confirmed);

            // Assert
            Assert.That(actualReport.Death, Is.Null);
            Assert.That(actualReport.Recovered, Is.Null);
        }

        [Test]
        public void ToReportType_OneReport_ReturnsRecoveredCaseReport()
        {
            // Arrange
            var fixture = new Fixture();
            var report = fixture.Create<CaseReport>();

            // Act
            var actualReport = report.ToReportType(ReportType.Recovered);

            // Assert
            Assert.That(actualReport.Death, Is.Null);
            Assert.That(actualReport.Confirmed, Is.Null);
        }

        [Test]
        public void ToReportType_MultipleReports_ReturnsAllCaseReports()
        {
            // Arrange
            var fixture = new Fixture();
            var reports = fixture.Create<IEnumerable<CaseReport>>();

            // Act
            var actualReports = reports.ToReportType(ReportType.All);

            // Assert
            Assert.That(actualReports.Where(x => x.Death != null && x.Recovered != null && x.Confirmed != null), 
                Is.EqualTo(reports));
        }

        [Test]
        public void ToReportType_MultipleReports_ReturnsDeathCaseReports()
        {
            // Arrange
            var fixture = new Fixture();
            var reports = fixture.Create<IEnumerable<CaseReport>>();

            // Act
            var actualReports = reports.ToReportType(ReportType.Death);

            // Assert
            Assert.That(actualReports.Where(x => x.Recovered is null && x.Confirmed is null), Is.EqualTo(reports));
        }

        [Test]
        public void ToReportType_MultipleReports_ReturnsConfirmedCaseReports()
        {
            // Arrange
            var fixture = new Fixture();
            var reports = fixture.Create<IEnumerable<CaseReport>>();

            // Act
            var actualReports = reports.ToReportType(ReportType.Confirmed);

            // Assert
            Assert.That(actualReports.Where(x => x.Recovered is null && x.Death is null), Is.EqualTo(reports));
        }

        [Test]
        public void ToReportType_MultipleReports_ReturnsRecoveredCaseReports()
        {
            // Arrange
            var fixture = new Fixture();
            var reports = fixture.Create<IEnumerable<CaseReport>>();

            // Act

            var actualReports = reports.ToReportType(ReportType.Recovered);

            // Assert
            Assert.That(actualReports.Where(x => x.Death is null && x.Confirmed is null), Is.EqualTo(reports));
        }
    }
}
