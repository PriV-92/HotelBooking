using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Test.Helpers
{
    public static class BookingsHelper
    {
        /// <summary>
        /// Returns the first booking that matches the provided string(FirstName)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public static IWebElement? GetBookingByFirstname(this IWebDriver driver, string firstName)
        {
            var booking = driver.FindElement(By.Id("bookings"))
                            .FindElements(By.ClassName("row"))
                            .FirstOrDefault(booking => booking.FindElement(By.XPath("//div[1]/p")).Text == firstName);

            if (booking == null)
            {
                return default;
            }

            return booking;
        }

        /// <summary>
        /// Returns the first booking in the table.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static IWebElement? GetFirstBooking(this IWebDriver driver)
        {
            var booking = driver.FindElement(By.Id("bookings"))
                            .FindElements(By.ClassName("row"));

            if (booking == null)
            {
                return default;
            }

            return booking[1];
        }

        /// <summary>
        /// Returns a count of the bookings placed. 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static int GetCountOfBookings(this IWebDriver driver)
        {
            var bookings = driver.FindElement(By.Id("bookings"))
                            .FindElements(By.ClassName("row"));

            return bookings.Count;
        }
    }
}
