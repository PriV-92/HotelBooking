using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace HotelBooking.Test.PageObject
{
    public class BookingFormPage
    {
        protected IWebDriver driver;

        private readonly By FirstName = By.Id("firstname");
        private readonly By LastName = By.Id("lastname");
        private readonly By Price = By.Id("totalprice");
        private readonly By Deposit = By.Id("depositpaid");
        private readonly By Checkin = By.Id("checkin");
        private readonly By Checkout = By.Id("checkout");
        private readonly By saveButton = By.XPath("//*[@id=\"form\"]/div/div[7]/input");


        public BookingFormPage(IWebDriver driver)
        { 
            this.driver = driver;
        }

        /// <summary>
        /// Enters a string value into the FirstName field.
        /// </summary>
        /// <param name="firstName"></param>
        public void EnterFirstName(string firstName)
        {
            driver.FindElement(FirstName).SendKeys(firstName);
        }

        /// <summary>
        /// Enters a string value into the LastName field.
        /// </summary>
        /// <param name="lastName"></param>
        public void EnterLastName(string lastName)
        {
            driver.FindElement(LastName).SendKeys(lastName);
        }

        /// <summary>
        /// Enters a string value into the Price field.
        /// </summary>
        /// <param name="price"></param>
        public void EnterPrice(string price)
        {
            driver.FindElement(Price).SendKeys(price);
        }

        /// <summary>
        /// Selects if deposit has been paid based on a bool value. 
        /// </summary>
        /// <param name="deposit"></param>
        public void SelectDeposit(bool deposit)
        {
            var depositSelector = new SelectElement(driver.FindElement(Deposit));
            depositSelector.SelectByText(deposit.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Selects a Check-In date based on a string input. Format: YYYY-MM-DD
        /// </summary>
        /// <param name="checkin"></param>
        public void SelectCheckinDate(string checkin)
        {
            driver.FindElement(Checkin).SendKeys(checkin);
        }

        /// <summary>
        /// Selects a Check-Out date based on a string input. Format: YYYY-MM-DD
        /// </summary>
        /// <param name="checkout"></param>
        public void SelectCheckoutDate(string checkout)
        {
            driver.FindElement(Checkout).SendKeys(checkout);
        }

        public void SubmitForm()
        {
            driver.FindElement(saveButton).Click();
        }

        public static IList<IWebElement> GetLastBooking(IWebDriver driver)
        {
            IList <IWebElement> booking = driver.FindElement(By.Id("bookings"))
                                                    .FindElements(By.ClassName("row"))
                                                    .Last()
                                                    .FindElements(By.TagName("p"));
            return booking;
        }
    }
}
