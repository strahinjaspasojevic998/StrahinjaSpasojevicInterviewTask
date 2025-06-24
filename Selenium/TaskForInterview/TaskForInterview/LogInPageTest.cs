using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TaskForInterview
{
    [TestFixture] 
    internal class LogInPageTest
    {
        private  string url = "https://www.saucedemo.com/";
        private IWebDriver driver;

        //elements
        private string usernameId = "user-name";
        private string passwordId = "password";
        private string loginButtonId = "login-button";
        private string errorMessageXPath = "//div[@class='error-message-container error']/h3[@data-test='error']"; //I would create ticket for adding Id for this element.

        //users
        private string usernameStandardUser = "standard_user";

        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito"); //I had issue with Google password manager so I used Incognito mode
            this.driver = new ChromeDriver(options);
            this.driver.Manage().Window.Maximize();
            this.driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void TearDown()
        {
            this.driver.Close();
        }

        [Test]
        public void TestLogInWithoutUsernameAndPassword()
        {
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            IWebElement errorMessageElement = this.driver.FindElement(By.XPath(errorMessageXPath));
            Assert.That(errorMessageElement.Text, Is.EqualTo ("Epic sadface: Username is required"), "Expected error message is not dispalyed.");
        }

        [Test]
        public void TestLoginWithValidUserNameAndNoPassword()
        {
            this.driver.FindElement(By.Id(usernameId)).SendKeys(usernameStandardUser);
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            IWebElement errorMessageElement = this.driver.FindElement(By.XPath(errorMessageXPath));
            Assert.That(errorMessageElement.Text, Is.EqualTo("Epic sadface: Password is required"), "Expected error message is not dispalyed.");
        }

        [Test]
        public void TestLoginWithInvalidUsername()
        {
            this.driver.FindElement(By.Id(usernameId)).SendKeys("Invalid Username");
            this.driver.FindElement(By.Id(passwordId)).SendKeys("secret_sauce");
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            IWebElement errorMessageElement = this.driver.FindElement(By.XPath(errorMessageXPath));
            Assert.That(errorMessageElement.Text, Is.EqualTo("Epic sadface: Username and password do not match any user in this service"), "Expected error message is not dispalyed.");
        }

        [Test]
        public void TestLoginWithInvalidPassword()
        {
            this.driver.FindElement(By.Id(usernameId)).SendKeys(usernameStandardUser);
            this.driver.FindElement(By.Id(passwordId)).SendKeys("Invalid password");
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            IWebElement errorMessageElement = this.driver.FindElement(By.XPath(errorMessageXPath));
            Assert.That(errorMessageElement.Text, Is.EqualTo("Epic sadface: Username and password do not match any user in this service"), "Expected error message is not dispalyed.");
        }

        [TestCase("standard_user")]
        [TestCase("problem_user")]
        [TestCase("performance_glitch_user")]
        [TestCase("error_user")]
        [TestCase("visual_user")]
        public void TestValidLogIn(string username)
        {
            this.driver.FindElement(By.Id(usernameId)).SendKeys(username);
            this.driver.FindElement(By.Id(passwordId)).SendKeys("secret_sauce");
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            string currentURL = this.driver.Url;
            Assert.That(currentURL, Is.EqualTo("https://www.saucedemo.com/inventory.html"), "URL is not correct.");
        }

        [Test]
        public void TestLoginWithLockedOutUser()
        {
            this.driver.FindElement(By.Id(usernameId)).SendKeys("locked_out_user");
            this.driver.FindElement(By.Id(passwordId)).SendKeys("secret_sauce");
            this.driver.FindElement(By.Id(loginButtonId)).Click();
            var errorMessageElement = this.driver.FindElement(By.XPath(errorMessageXPath));
            Assert.That(errorMessageElement.Text, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."), "Expected error message is not dispalyed.");
        }
    }
}
