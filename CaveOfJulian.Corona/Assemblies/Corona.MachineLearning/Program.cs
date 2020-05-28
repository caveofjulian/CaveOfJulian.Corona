using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Corona.DataIngestion;
using Corona.MachineLearning.Data;
using Corona.Persistence;
using Corona.Persistence.Entities;
using Corona.Services;
using Corona.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;

namespace Corona.MachineLearning
{
    internal class Program
    {
        private static readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDb;Database=RoniCoronaDb;Trusted_Connection=True;";

        private static readonly string _selectQuery = @"
        SELECT TOP (*) [Country]
      ,[Date]
      ,CAST([Confirmed] as REAL) as Confirmed
      ,CAST([Death] as REAL) as Death
      ,CAST([Recovered] as REAL) as Recovered
  FROM [RoniCoronaDb].[dbo].[MLCases]
";

        private static void Main(string[] args)
        {
            string rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
            string modelPath = Path.Combine(rootDir, "MLModel.zip");

            var context = new MLContext();
            var loader = context.Data.CreateDatabaseLoader<ModelInput>();
            var dbSource = new DatabaseSource(SqlClientFactory.Instance, _connectionString, _selectQuery);
            var dataView = loader.Load(dbSource);

            var pipeline = context.Forecasting.ForecastBySsa(
                outputColumnName: "ForcastedDeath",
                inputColumnName: "Death",
                windowSize:7,
                seriesLength:30,
                trainSize:365,
                horizon:1,
                confidenceLevel:0.95f,
                confidenceLowerBoundColumn:"LowerBoundDeath",
                confidenceUpperBoundColumn:"UpperBoundDeath"
                );



            var forecaster = pipeline.Fit(dataView);

            var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(context);
            forecastEngine.CheckPoint(context, modelPath);

            Forecast(dataView, 23, forecastEngine, context);

            Console.ReadKey();
        }

        private static void Ingest()
        {
            var logger = new Logger<MachineLearningIngester>(new LoggerFactory());

            var optionsBuilder = new DbContextOptionsBuilder<RoniContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            var context = new RoniContext(optionsBuilder.Options);

            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());
            var mapper = new Mapper(config);

            var service = new CoronaService(new CoronaCasesRepository<Cases>(context), mapper);

            using (var ingester = new MachineLearningIngester(logger, new SqlConnection(_connectionString), service))
            {
                ingester.Ingest(new DateTime(2020, 1, 22));
            }
        }

        private static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            // Make predictions
            var predictions = model.Transform(testData);

            // Actual values
            IEnumerable<float> actual =
                mlContext.Data.CreateEnumerable<ModelInput>(testData, true)
                    .Select(observed => observed.Death);

            // Predicted values
            IEnumerable<float> forecast =
                mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true)
                    .Select(prediction => prediction.ForcastedDeath[0]);

            // Calculate error (actual - forecast)
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            // Get metric averages
            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            // Output metrics
            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }

        private static void Forecast(IDataView testData, int horizon,
            TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
        {
            var forecast = forecaster.Predict();

            var forecastOutput =
                mlContext.Data.CreateEnumerable<ModelInput>(testData, false)
                    .Take(horizon)
                    .Select((rental, index) =>
                    {
                        string rentalDate = rental.Date.ToShortDateString();
                        float actualDeaths = rental.Death;
                        float lowerEstimate = Math.Max(0, forecast.LowerBoundDeath[index]);
                        float estimate = forecast.ForcastedDeath[index];
                        float upperEstimate = forecast.UpperBoundDeath[index];
                        return $"Date: {rentalDate}\n" +
                               $"Actual Deaths: {rentalDate}\n" +
                               $"Lower Estimate: {lowerEstimate}\n" +
                               $"Forecast: {estimate}\n" +
                               $"Upper Estimate: {upperEstimate}\n";
                    });

            Console.WriteLine("Corona Forecast");
            Console.WriteLine("---------------------");
            foreach (var prediction in forecastOutput) Console.WriteLine(prediction);
        }
    }
}