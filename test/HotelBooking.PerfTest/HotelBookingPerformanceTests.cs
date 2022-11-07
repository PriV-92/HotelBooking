using HotelBooking.PerfTest.Models;
using NBomber.Contracts;
using Newtonsoft.Json;

namespace HotelBooking.PerfTest
{
    public class HotelBookingPerformanceTests
    {
        [Test]
        public void GIVEN_increasing_load_WHEN_getting_bookings_THEN_latency_must_be_below_1000ms()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://hotel-test.equalexperts.io/booking");

            //Arrange
            var bookingData = new BookingModel
            {
                firstname = "test",
                lastname = "test",
                totalprice = "1",
                depositpaid = "true",
                bookingdates = new DateModel
                {
                    checkin = "2022-11-14",
                    checkout = "2022-11-22"
                }
            };

            var step = Step.Create("place_booking_step", async context =>
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = httpClient.BaseAddress,
                    Content = new StringContent(JsonConvert.SerializeObject(bookingData), System.Text.Encoding.UTF8, "application/json")

                };
                var result = await httpClient.SendAsync(request);
                return Response.Ok(statusCode: (int)result.StatusCode);
            });

            var scenario = ScenarioBuilder
                .CreateScenario("place_bookings_scenario", step)
                .WithoutWarmUp()
                .WithLoadSimulations(
                //Reduce run times for running on pipeline due to multiple users on the site. 
                //Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(60)),
                //Simulation.KeepConstant(copies: 2, during: TimeSpan.FromSeconds(60)),
                //Simulation.RampConstant(copies: 10,during: TimeSpan.FromSeconds(60))
                Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(10)),
                Simulation.KeepConstant(copies: 2, during: TimeSpan.FromSeconds(10)),
                Simulation.RampConstant(copies: 10, during: TimeSpan.FromSeconds(10))
            );

            //Act
            var stats = NBomberRunner
                .RegisterScenarios(scenario)
                .WithTestName("HotelBookingPerformance")
                .WithTestSuite("PerformanceTest")
                .WithReportFileName("HotelBooking")
                //TODO - Add runsettings for local pathing.
                //.WithReportFolder("../../../../../TestResults/Performance")
                .WithReportFolder("./TestResults/NBomber")
                .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
                .Run();

            //Assert
            var stepStats = stats.ScenarioStats[0].StepStats[0].Ok;
            stepStats.Latency.MaxMs.Should().BeLessThan(1000);
            stepStats.Latency.MeanMs.Should().BeLessThan(800);
        }
    }
}