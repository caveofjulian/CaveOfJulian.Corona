using System;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Roni.Corona.Persistence;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Shared;

namespace Roni.Corona.Services.Tests
{
    public class CoronaServiceTests
    {
        [Test]
        public void GetLastUpdated_ReturnsLastUpdated()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new []
            {
                new Cases() {LastUpdated = new DateTime(2020,12,5)},
                new Cases() {LastUpdated = new DateTime(2000,11,6)},
                new Cases() {LastUpdated = new DateTime(2007,2,7)},
                new Cases() {LastUpdated = new DateTime(2021,3,1)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
                repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var expected = data[1].Date;

            // Act
            var actual = service.GetLastUpdated();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetCases_All_ReturnsAllReports()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Death = 5, LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Confirmed = 20, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Death = 5, Confirmed = 0, Recovered = 0, LastUpdated = new DateTime(2020,12,5)},
                new CaseReport() {Death = 10, Confirmed = 20, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters();

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.All);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_Deaths_ReturnsDeathReports()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Death = 5, LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Confirmed = 20, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Death = 5, Confirmed = null,LastUpdated = new DateTime(2020,12,5)},
                new CaseReport() {Death = 10, Confirmed = null, Recovered = null, LastUpdated = new DateTime(2000,11,6)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters();

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Death);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_Confirmed_ReturnsConfirmedReports()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Confirmed = 5, LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 20, Confirmed = 10, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Confirmed = 5, LastUpdated = new DateTime(2020,12,5)},
                new CaseReport() {Death = null, Confirmed = 10, Recovered = null, LastUpdated = new DateTime(2000,11,6)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters();

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Confirmed);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_Recovered_ReturnsRecoveredReports()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 20, Confirmed = 30, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = 5, LastUpdated = new DateTime(2020,12,5)},
                new CaseReport() {Death = null, Confirmed = null, Recovered = 10, LastUpdated = new DateTime(2000,11,6)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters();

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Recovered);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_DeathReportsForDate_ReturnsReportsForDate()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, Date = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Date = new DateTime(2020,12,5)},
                new Cases() {Death = 20, Confirmed = 30, Recovered = 10, Date = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = null, Confirmed = null, Death = 0, Date = new DateTime(2020,12,5)},
                new CaseReport() {Recovered = null, Confirmed = null, Death = 10, Date = new DateTime(2020,12,5)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters()
            {
                Date = new DateTime(2020, 12, 5)
            };

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Death);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_ConfirmedReportsForDateRange_ReturnsReportsForDateRange()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, Date = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Date = new DateTime(2020,12,7)},
                new Cases() {Death = 20, Confirmed = 30, Recovered = 10, Date = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = null, Confirmed = 0, Death = null, Date = new DateTime(2020,12,5)},
                new CaseReport() {Recovered = null, Confirmed = 0, Death = null, Date = new DateTime(2020,12,7)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters()
            {
                BeginDate = new DateTime(2020, 12, 5),
                EndDate = new DateTime(2020, 12, 7)
            };

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Confirmed);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }


        [Test]
        public void GetCases_RecoveredReportsForCountry_ReturnsReportsForCountry()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, Country = "UK" ,Date = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Country = "UK", Date = new DateTime(2020,12,9)},
                new Cases() {Death = 20, Country = "NL", Confirmed = 30, Recovered = 10, Date = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = 5, Confirmed = null, Death = null, Country = "UK", Date = new DateTime(2020,12,5)},
                new CaseReport() {Recovered = 0, Confirmed = null, Death = null, Country = "UK", Date = new DateTime(2020,12,9)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters()
            {
                Country = "UK",
            };

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.Recovered);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_AllReportsForCountryAndDate_ReturnsReportsForCountryAndDate()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, Country = "UK" ,Date = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Country = "UK", Date = new DateTime(2020,12,5)},
                new Cases() {Death = 20, Country = "NL", Confirmed = 30, Recovered = 10, Date = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = 5, Confirmed = 0, Death = 0, Country = "UK", Date = new DateTime(2020,12,5)},
                new CaseReport() {Recovered = 0, Confirmed = 0, Death = 10, Country = "UK", Date = new DateTime(2020,12,5)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));
            
            var coronaParameters = new CoronaParameters()
            {
                Country = "UK",
                Date = new DateTime(2020, 12, 5)
            };
            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.All);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetCases_AllReportsForCountryAndDateRange_ReturnsReportsForCountryAndDateRange()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() {Recovered = 5, Country = "UK" ,Date = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Country = "UK", Date = new DateTime(2020,12,7)},
                new Cases() {Death = 20, Country = "NL", Confirmed = 30, Recovered = 10, Date = new DateTime(2000,11,6)},
            };

            var expectedData = new[]
            {
                new CaseReport() {Recovered = 5, Confirmed = 0, Death = 0, Country = "UK", Date = new DateTime(2020,12,5)},
                new CaseReport() {Recovered = 0, Confirmed = 0, Death = 10, Country = "UK", Date = new DateTime(2020,12,7)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            var coronaParameters = new CoronaParameters()
            {
                Country ="UK",
                BeginDate = new DateTime(2020, 12, 5),
                EndDate = new DateTime(2020, 12, 7)
            };

            // Act
            var actualData = service.GetCases(coronaParameters, ReportType.All);

            // Assert
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void GetTotalDeathCases_ReturnsTotalDeathCases()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() { Death = 5, LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Confirmed = 20, LastUpdated = new DateTime(2000,11,6)},
                new Cases() {Death = 10, Confirmed = 20, Recovered = 30, LastUpdated = new DateTime(2007,2,7)},
                new Cases() { LastUpdated = new DateTime(2021,3,1)},
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            // Act
            var actual = service.GetTotalDeathCases();

            // Assert
            Assert.That(actual, Is.EqualTo(25));
        }

        [Test]
        public void GetTotalConfirmedCases_ReturnsTotalConfirmedCases()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() { Death = 5, Confirmed = 10,LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Confirmed = 20, LastUpdated = new DateTime(2000,11,6)},
                new Cases() {Death = 10, Confirmed = 20, Recovered = 30, LastUpdated = new DateTime(2000,11,6)},
                new Cases() { LastUpdated = new DateTime(2021,3,1)}
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            // Act
            var actual = service.GetTotalConfirmedCases();

            // Assert
            Assert.That(actual, Is.EqualTo(50));
        }

        [Test]
        public void GetTotalRecoveredCases_ReturnsTotalRecoveredCases()
        {
            // Arrange
            var fixture = new Fixture();

            var data = new[]
            {
                new Cases() { Death = 5, Confirmed = 10,LastUpdated = new DateTime(2020,12,5)},
                new Cases() {Death = 10, Recovered = 20, LastUpdated = new DateTime(2000,11,6)},
                new Cases() {Death = 10, Confirmed = 20, Recovered = 30, LastUpdated = new DateTime(2000,11,6)},
                new Cases() { LastUpdated = new DateTime(2021,3,1)}
            };

            var repository = new Mock<ICoronaCasesRepository<Cases>>();
            repository
                .Setup(x => x.Get())
                .Returns(data.AsQueryable());

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());

            var service = new CoronaService(repository.Object, new Mapper(config));

            // Act
            var actual = service.GetTotalRecoveredCases();

            // Assert
            Assert.That(actual, Is.EqualTo(50));
        }


    }
}