using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using WebApi.Hubs;
using WebApi.Models;
using Xunit;
using System;

namespace WebAPI.Tests
{
    public class MeasurementControllerTests
    {
        

        [Fact]
        public async void test()
        {
            var connection = new SqliteConnection("Data Source =:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MeasurementDbContext>().UseSqlite(connection).Options;

            using (var context = new MeasurementDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Locations.AddRange(
                    new Location { LocationId = 1, Name = "Hornslet", Longitude = 1, Latitude = 2 }
                    );
                context.Measurements.AddRange(
                    new Measurement { MeasurementId = 2, DateNTime = DateTime.Now, AirMoisture = 2, AirPressure = 3, Temperature = 0, LocationId = 1 }
                    );
                    
                context.SaveChanges();
            }
            using (var context = new MeasurementDbContext(options))
            {
                var service = new MeasurementController(context, null);
                var measurement = service.Get(id: 1);
                Assert.NotNull(measurement);
                Assert.Equal(1, measurement.Id);
                Assert.Equal(0, measurement.Result.Value[1].Temperature);
            }


        }
    }
}


////Arrange
//var fakeMesurment = A.CollectionOfDummy<Measurement>(5);
//fakeMesurment[0].MeasurementId = 1;
//fakeMesurment[0].AirMoisture = 12;
//fakeMesurment[0].AirPressure = 10;
//fakeMesurment[0].Temperature = 22;
//fakeMesurment[0].DateNTime = DateTime.Now;

//var DataStore = A.Fake<IMeasurementDbContext>();
//var notifyHubContext = A.Fake<IHubContext<NotificationHub, INotification>>();
//MeasurementController controller = new MeasurementController(DataStore, notifyHubContext);
////act 
//var actionRes = await controller.Get(1);
//var result = actionRes.Result as OkObjectResult;
//var returnMeasurements = result.Value as IEnumerable<Measurement>;

//// assert
//Assert.Equal();