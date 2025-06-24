using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TaskForInterview
{
    [TestFixture]
    internal class CartPageTest
    {
        public string url = "https://www.saucedemo.com/";
        public IWebDriver driver;
        //elements
        public string shoppingCartBadgeMessageXPath = "//div[@id='shopping_cart_container']/a[@class='shopping_cart_link']/span[@class='shopping_cart_badge']";
        public string buttonForAddingItemSauceLabsBackpackId = "add-to-cart-sauce-labs-backpack";

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            this.driver = new ChromeDriver(options);
            this.driver.Manage().Window.Maximize();
            this.driver.Navigate().GoToUrl(url);
            this.LogIn();

        }

        private void LogIn()
        {
            this.driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            this.driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            this.driver.FindElement(By.Id("login-button")).Click();
            string currentURL = this.driver.Url;
            Assert.That(currentURL, Is.EqualTo("https://www.saucedemo.com/inventory.html"), "URL is not correct.");
        }
        
        [TearDown]
        public void TearDown()
        {
            this.driver.Close();
        }
        [Test]

        public void TestAddingOneItemInCart()
        {

            this.driver.FindElement(By.Id(buttonForAddingItemSauceLabsBackpackId)).Click();
            IWebElement cartItemCount = this.driver.FindElement(By.XPath(shoppingCartBadgeMessageXPath));
            Assert.That(cartItemCount.Text, Is.EqualTo("1"), "The number of items is different then expected.");
        }
        [Test]

        public void TestAddingMultipleItemsInCart()
        {

            this.driver.FindElement(By.Id(buttonForAddingItemSauceLabsBackpackId)).Click();
            this.driver.FindElement(By.Id("add-to-cart-sauce-labs-bike-light")).Click();
            this.driver.FindElement(By.Id("add-to-cart-sauce-labs-bolt-t-shirt")).Click();
            IWebElement cartItemCount = this.driver.FindElement(By.XPath(shoppingCartBadgeMessageXPath));
            Assert.That(cartItemCount.Text, Is.EqualTo("3"), "The number of items is different then expected.");
            cartItemCount.Click();
            ReadOnlyCollection<IWebElement> cartListOfAddedItems = this.driver.FindElements(By.XPath("//div[@class='cart_item']"));
            Assert.That(cartListOfAddedItems.Count, Is.EqualTo(3), "The number of items is different then expected.");

        }
        [Test]

        public void TestRemovingItemFromTheCart()
        {
            this.driver.FindElement(By.Id(buttonForAddingItemSauceLabsBackpackId)).Click();
            IWebElement cartItemCount = this.driver.FindElement(By.XPath(shoppingCartBadgeMessageXPath));
            Assert.That(cartItemCount.Text, Is.EqualTo("1"), "You have different number of items in cart!");
            this.driver.FindElement(By.Id("remove-sauce-labs-backpack")).Click();
            IWebElement cartItemIcon = this.driver.FindElement(By.XPath("//div[@id='shopping_cart_container']/a[@class='shopping_cart_link']"));
            Assert.That(cartItemIcon.Text, Is.EqualTo(""), "You have different number of items in cart!");
            cartItemIcon.Click();
            ReadOnlyCollection<IWebElement> cartListOfAddedItems = this.driver.FindElements(By.XPath("//div[@class='cart_item']"));
            Assert.That(cartListOfAddedItems.Count, Is.EqualTo(0), "The number of items is different then expected.");

        }
    }
}
