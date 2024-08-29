using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ecommerce.selenium
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AcceptInsecureCertificates = true;  
            driver = new ChromeDriver(chromeOptions);
        }

        [Test]
        public void OpenHomePage()
        {
            driver.Navigate().GoToUrl("https://localhost:44336/");
            Assert.AreEqual("Home Page", driver.Title);
        }

        [TearDown]
        public void Teardown()
        {
            // Close the browser and clean up
            driver.Quit();
        }
    }
}