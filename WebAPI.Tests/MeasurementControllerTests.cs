using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using WebApi.Hubs;
using WebApi.Models;
using Xunit;
using System;
using System.Net;

namespace WebAPI.Tests
{
    public class MeasurementControllerTests
    {
        public DbContextOptions<MeasurementDbContext> options { get; private set; }
        Location L1 = new Location(1, "Hornslet", 1, 2);
        private void createfakeDatabase()
        {
            var connection = new SqliteConnection("Data Source =:memory:");
            connection.Open();
             options = new DbContextOptionsBuilder<MeasurementDbContext>().UseSqlite(connection).Options;

            using (var context = new MeasurementDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Measurements.AddRange(
                    new Measurement { MeasurementId = 1, DateNTime = DateTime.Now, AirMoisture = 2, AirPressure = 3, Temperature = 0, LocationId = 1, Location = L1 },
                    new Measurement { MeasurementId = 2, DateNTime = DateTime.Now, AirMoisture = 3, AirPressure = 4, Temperature = 6, LocationId = 1, Location = L1 },
                    new Measurement { MeasurementId = 3, DateNTime = DateTime.Now, AirMoisture = 1, AirPressure = 1, Temperature = 2, LocationId = 1, Location = L1 },
                    new Measurement { MeasurementId = 4, DateNTime = DateTime.Now, AirMoisture = 6, AirPressure = 3.3, Temperature = 8, LocationId = 1, Location = L1 }
                    ) ;

                context.SaveChanges();
            }
        }
      
        [Fact]
        public async void MeasurementController_getId_returnMeasurent()
        {
           createfakeDatabase();
            using (var context = new MeasurementDbContext(options))
            {
                var service = new MeasurementController(context, null);
                var measurement = service.Get(id: 1);
                Assert.NotNull(measurement);
                Assert.Equal(1, measurement.Result.Value[0].MeasurementId);
                Assert.Equal(0, measurement.Result.Value[0].Temperature);
                Assert.Equal(2, measurement.Result.Value[0].AirMoisture);
                Assert.Equal(3, measurement.Result.Value[0].AirPressure);
                Assert.Equal(1, measurement.Result.Value[0].LocationId);
            }
        }
        [Fact]
        public async void MeasurmentController_get3lastest_return3measurments()
        {
            createfakeDatabase();
            using (var context = new MeasurementDbContext(options))
            {
                var service = new MeasurementController(context, null);
                var measurement = service.GetLatest();
                Assert.NotNull(measurement);
                Assert.Equal(3, measurement.Result.Value.Count);
           
                Assert.Equal(4, measurement.Result.Value[0].MeasurementId);
                Assert.Equal(3, measurement.Result.Value[1].MeasurementId);
                Assert.Equal(2, measurement.Result.Value[2].MeasurementId);

            }
        }

        [Fact]
        public async void MeasurmentController_getMeasurementOnDate_returnmeasurments()
        {
            createfakeDatabase();
            using (var context = new MeasurementDbContext(options))
            {
                var service = new MeasurementController(context, null);

                var measurement = service.GetAtDate(DateTime.Now);
                Assert.NotNull(measurement);
                Assert.Equal(4, measurement.Result.Value.Count);
            }
        }

        [Fact]
        public async void MeasurmentController_postMeasurement_MeasurmentPosted()
        {
            createfakeDatabase();
            using (var context = new MeasurementDbContext(options))
            {
                var service = new MeasurementController(context, null);
                Measurement m1 = new Measurement { MeasurementId = 5, AirMoisture = 2, DateNTime = DateTime.Now, Temperature = 0, AirPressure = 2, LocationId = 1};
                var measurement = service.Post(m1);
                var measurementGet = service.GetMeasurements();
                Assert.Equal(5, measurementGet.Result.Value.Count);
                Assert.Equal(5, measurementGet.Result.Value[4].MeasurementId);
                Assert.Equal(2, measurementGet.Result.Value[4].AirPressure);
                Assert.Equal(2, measurementGet.Result.Value[4].AirMoisture);
                Assert.Equal(0, measurementGet.Result.Value[4].Temperature);
                Assert.Equal(1, measurementGet.Result.Value[4].LocationId);

            }
        }
    }
}


