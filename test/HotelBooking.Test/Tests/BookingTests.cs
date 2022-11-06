using HotelBooking.Test.Helpers;
using HotelBooking.Test.PageObject;
using OpenQA.Selenium.Support.UI;

namespace HotelBooking.Test.Tests
{
    class BookingTests
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void StartBrowser()
        {
            driver = new ChromeDriver
            {
                Url = "http://hotel-test.equalexperts.io/"
            };

            wait = new(driver, timeout: TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromSeconds(1),
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(driver => driver.FindElement(By.Id("form")));
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void GIVEN_hotel_booking_form_WHEN_page_loads_THEN_page_title_is_correct()
        {
            //Arrange
            string expectedTitle = "Hotel booking form";
            string currentTitle;

            //Act
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(x => x.FindElement(By.Id("form")));

            currentTitle = driver.Title;

            driver.Close();

            //Assert
            currentTitle.Should().Be(expectedTitle);
        }

        [Test]
        public void GIVEN_hotel_booking_form_WHEN_form_submitted_THEN_booking_is_placed()
        {
            //Arrange
            var BookingForm = new BookingFormPage(driver);
            int currentBookings = driver.GetCountOfBookings();

            //Act
            BookingForm.EnterFirstName("Bob");
            BookingForm.EnterLastName("d");
            BookingForm.EnterPrice("100");
            BookingForm.SelectDeposit(true);
            BookingForm.SelectCheckinDate("2022-12-01");
            BookingForm.SelectCheckoutDate("2022-12-02");
            BookingForm.SubmitForm();

            //Assert
            //Wait is required for the page to be updated. >10seconds will fail.
            wait.Until(driver => driver.GetCountOfBookings() > currentBookings);

            var lastBooking = BookingFormPage.GetLastBooking(driver);
            lastBooking[0].Text.Should().Be("Bob");
            lastBooking[1].Text.Should().Be("d");
            lastBooking[2].Text.Should().Be("100");
            lastBooking[3].Text.Should().Be("true");
            lastBooking[4].Text.Should().Be("2022-12-01");
            lastBooking[5].Text.Should().Be("2022-12-02");
        }

        [Test]
        public void GIVEN_hotel_bookings_WHEN_first_booking_deleted_THEN_booking_is_removed()
        {
            //Arrange
            //Wait is required as the booking table loads after the page has completed loading. 
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait?.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bookings\"]/div[2]/div[1]/p")));

            //Act
            int currentBookings = driver.GetCountOfBookings();
            var booking = driver.GetFirstBooking();
            booking?.FindElement(By.XPath("//div[7]/input")).Click();

            //Assert
            Thread.Sleep(1000);
            int updatedtBookings = driver.GetCountOfBookings();

            currentBookings.Should().BeGreaterThan(updatedtBookings);
        }

    }
}