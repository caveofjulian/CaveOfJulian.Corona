using System;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Roni.Corona.Api.Controllers;
using Roni.Corona.Services;
using Roni.Corona.Shared;

namespace Roni.Corona.Api.UnitTests
{
    public class CoronaControllerTests
    {
        [Test]
        public void Get_All_ReturnsAllCountry()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(), It.IsAny<string>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters()
            {
                Country = "Netherlands"
            };

            // Act
            var result = controller.Get(parameters) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }

        [Test]
        public void Get_CountrySpecificDateRanges_ReturnsAllCases()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(), 
                    It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters()
            {
                Country = "Netherlands",
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            // Act
            var result = controller.Get(parameters) as OkObjectResult;
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }

        [Test]
        public void Get_CountrySpecificOnDate_ReturnsAllCases()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(),
                    It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters()
            {
                Country = "Netherlands",
                Date = DateTime.Now,
            };

            // Act
            var result = controller.Get(parameters) as OkObjectResult;
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }

        [Test]
        public void Get_AllCountryDeaths_ReturnsDeaths()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(), It.IsAny<string>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters();

            // Act
            var result = controller.GetDeathCases(parameters) as OkObjectResult;
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }

        [Test]
        public void Get_AllCountryDeaths_ReturnsConfirmed()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(), It.IsAny<string>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters()
            {
                Country = "Netherlands"
            };

            // Act
            var result = controller.GetConfirmedCases(parameters) as OkObjectResult;
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }

        [Test]
        public void Get_AllCountryDeaths_ReturnsRecovered()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedReports = fixture.Create<CaseReport[]>();

            var service = new Mock<ICoronaService>();
            service
                .Setup(x => x.GetCases(It.IsAny<ReportType>(), It.IsAny<string>()))
                .Returns(expectedReports);

            var controller = new CoronaController(service.Object, It.IsAny<ILogger>());

            var parameters = new CoronaParameters()
            {
                Country = "Netherlands"
            };

            // Act
            var result = controller.GetRecoveredCases(parameters) as OkObjectResult;
            var actualReports = result.Value;

            // Assert
            Assert.That(actualReports, Is.EqualTo(expectedReports));
        }
    }
}